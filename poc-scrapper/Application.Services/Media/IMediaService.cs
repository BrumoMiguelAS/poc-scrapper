namespace Scrapper.Application.Services.Media
{
    using System.Threading.Tasks;
    using Scrapper.Application.Dto.Media;

    public interface IMediaService
    {
        Task<Media> InsertMediaAsync(Media media);
    }
}
