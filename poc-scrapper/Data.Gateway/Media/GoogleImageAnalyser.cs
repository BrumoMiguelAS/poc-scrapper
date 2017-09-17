namespace Scrapper.Data.Gateway.Media
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Model.Media;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Cloud.Vision.V1;

    public class GoogleImageAnalyser : IImageAnalyser
    {
        private readonly string mediaStoragePath;

        public GoogleImageAnalyser(
            string mediaStoragePath)
        {
            this.mediaStoragePath = mediaStoragePath;
        }

        public async Task<IEnumerable<Media>> AnalyseAsync(IEnumerable<Media> mediaList)
        {
            var analysedMedia = new List<Media>();
            await ConnectToGoogleApiAccountAsync();

            var client = ImageAnnotatorClient.Create();
            foreach (var media in mediaList)
            {
                var analysedImage = this.AnalyseImage(client, media, $"{mediaStoragePath}/{media.PhysicalPath}");
                if (analysedImage != null)
                {
                    analysedMedia.Add(analysedImage);
                }
            }

            return analysedMedia;
        }

        private Media AnalyseImage(ImageAnnotatorClient client, Media media, string imagePath)
        {
            var image = Image.FromFile(imagePath);

            var safeSearch = client.DetectSafeSearch(image);
            if (safeSearch.Adult == Likelihood.Likely || safeSearch.Adult == Likelihood.VeryLikely ||
               safeSearch.Violence == Likelihood.Likely || safeSearch.Violence == Likelihood.VeryLikely)
            {
                return null;
            }

            var newMedia = media;
            newMedia.Status = MediaStatus.Approved;

            var colorList = this.AnalyseColors(client, image);
            var facesList = this.AnalyseFaces(client, image);
            var labelsList = this.AnalyseLabels(client, image);

            if (colorList.Any())
            {
                newMedia.Properties.AddRange(colorList);
            }

            if (facesList.Any())
            {
                newMedia.Properties.AddRange(facesList);
            }

            if (labelsList.Any())
            {
                newMedia.Properties.AddRange(labelsList);
            }

            return newMedia;
        }

        private List<MediaProperties> AnalyseColors(ImageAnnotatorClient client, Image image)
        {
            var colorList = new List<MediaProperties>();
            var imageProperties = client.DetectImageProperties(image);

            if (imageProperties != null && imageProperties.DominantColors != null && imageProperties.DominantColors.Colors != null)
            {
                foreach (var color in imageProperties.DominantColors.Colors)
                {
                    colorList.Add(new MediaProperties
                    {
                        Type = MediaPropertyType.Color,
                        Value = color.Color.Red + "" + color.Color.Green + "" + color.Color.Blue,
                        Score = color.Score,
                    });
                }
            }

            return colorList;
        }

        private List<MediaProperties> AnalyseFaces(ImageAnnotatorClient client, Image image)
        {
            var facesList = new List<MediaProperties>();
            var imageFaces = client.DetectFaces(image);

            //if (imageFaces != null && imageFaces.Count > 0)
            //{
            //    foreach (var face in imageFaces)
            //    {
            //        facesList.Add(new MediaProperties
            //        {
            //            Type = MediaPropertyType.Vips
            //        });
            //    }
            //}

            return facesList;
        }

        private List<MediaProperties> AnalyseLabels(ImageAnnotatorClient client, Image image)
        {
            var facesList = new List<MediaProperties>();
            var imageLabels = client.DetectText(image);

            if (imageLabels != null && imageLabels.Count > 0)
            {
                foreach (var label in imageLabels)
                {
                    facesList.Add(new MediaProperties
                    {
                        Type = MediaPropertyType.Labels,
                        Value = label.Description,
                        Score = label.Confidence
                    });
                }
            }

            return facesList;
        }


        private static async Task ConnectToGoogleApiAccountAsync()
        {
            var googleCredentials = await GoogleCredential.GetApplicationDefaultAsync();
            var compute = new ComputeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredentials
            });
        }
    }

    internal class ComputeService
    {
        private BaseClientService.Initializer initializer;

        public ComputeService(BaseClientService.Initializer initializer)
        {
            this.initializer = initializer;
        }
    }
}
