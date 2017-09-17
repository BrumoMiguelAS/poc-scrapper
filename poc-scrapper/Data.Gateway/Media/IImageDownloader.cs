namespace Scrapper.Data.Gateway.Media
{
    using System.Threading.Tasks;
    using Domain.Model.Media;

    public interface IImageDownloader
    {
        Task<Media> Download(string imageSource, string sourceName);
    }
}
