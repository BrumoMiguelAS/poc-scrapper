namespace Scrapper.Data.Gateway.Scrapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using Scrapper.Data.Gateway.Media;
    using Scrapper.Domain.Model.Scrapping.Sources;
    using Domain.Model.Media;
    using Scrapper.Data.Repository.Interfaces;

    public class ScrappingSourceProcessor : IScrappingSourceProcessor
    {
        private readonly IImageDownloader imageDownloader;
        private readonly IImageAnalyser imageAnalyser;
        private readonly IMediaRepository mediaRepository;

        public ScrappingSourceProcessor(
            IImageDownloader imageDonwloader,
            IImageAnalyser imageAnalyser,
            IMediaRepository mediaRepository)
        {
            this.imageDownloader = imageDonwloader;
            this.mediaRepository = mediaRepository;
            this.imageAnalyser = imageAnalyser;
        }

        public async Task<IEnumerable<Media>> ProcessAsync(ScrappingSource source)
        {
            try
            {
                var downloadedMedia = new List<Media>();

                var httpClient = new HttpClient();
                using (var response = await httpClient.GetAsync(source.Url))
                {
                    using (var content = response.Content)
                    {
                        var result = await content.ReadAsStringAsync();

                        var htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(result);

                        var imagesList = htmlDocument.DocumentNode.Descendants("img").Select(x => x);
                        downloadedMedia = await this.ProccessImagesAsync(imagesList, source.Name).ConfigureAwait(false);

                    }
                }

                var analysedMedia = await this.AnalyseDownloadedImagesAsync(downloadedMedia, source.Name).ConfigureAwait(false);
                await this.mediaRepository.AddManyAsync(analysedMedia.Where(m => m.Status != MediaStatus.PendingApproval)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Enumerable.Empty<Media>();
        }

        private async Task<List<Media>> AnalyseDownloadedImagesAsync(List<Media> downloadedMedia, string sourceName)
        {
            foreach (var media in downloadedMedia)
            {
                media.PhysicalPath = $"{sourceName}/{media.Name}";
                media.Status = MediaStatus.Approved;
            }

            //WIP: Add a common image analyser logic, lik google or microsoft. This will auto approve images based on their content
            //var analysedMedia = await this.imageAnalyser.AnalyseAsync(downloadedMedia).ConfigureAwait(false);
            //return analysedMedia.ToList();

            return downloadedMedia;
        }

        private async Task<List<Media>> ProccessImagesAsync(IEnumerable<HtmlNode> imagesList, string sourceName)
        {
            var downloadedMedia = new List<Media>();

            if (imagesList != null)
            {
                foreach (var image in imagesList)
                {
                    var srcAttribute = image.GetAttributeValue("src", "src");

                    // Ensure that only hosted images are served
                    if (srcAttribute.StartsWith("http"))
                    {
                        try
                        {
                            var media = await this.imageDownloader.Download(srcAttribute, sourceName).ConfigureAwait(false);
                            downloadedMedia.Add(media);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }

            return downloadedMedia.ToList();
        }
    }
}
