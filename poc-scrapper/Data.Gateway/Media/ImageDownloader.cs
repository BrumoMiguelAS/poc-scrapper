namespace Scrapper.Data.Gateway.Media
{
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Domain.Model.Media;

    public class ImageDownloader : IImageDownloader
    {
        private readonly string imagePath;

        public ImageDownloader(
            string imagePath)
        {
            this.imagePath = imagePath;
        }

        public async Task<Media> Download(string imageSource, string sourceName)
        {
            var image = new Media();

            var folderPath = this.CreateFolder($"{this.imagePath}/{sourceName}");
            var imagePath = $"{folderPath}/{imageSource.Split('/').LastOrDefault()}";

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, imageSource))
                {
                    using (
                        Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
                        stream = new FileStream(imagePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        await contentStream.CopyToAsync(stream);
                        image = this.CreateMedia(imageSource, (stream as FileStream).Name);

                    }
                }
            }

            return image;
        }

        private Media CreateMedia(string imageSource, string mediaName)
        {
            return new Media
            {
                Name = mediaName.Split('\\').LastOrDefault(),
                OriginalUrl = imageSource,
                Status = MediaStatus.PendingApproval,
                Type = MediaType.Image
            };
        }

        private string CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
    }
}
