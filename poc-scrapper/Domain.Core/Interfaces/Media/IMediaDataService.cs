namespace Scrapper.Domain.Core.Interfaces.Media
{
    using System.Threading.Tasks;
    using Scrapper.Domain.Model.Media;

    public interface IMediaDataService
    {
        Task<Media> InsertMediaAsync(Media media);
    }
}
