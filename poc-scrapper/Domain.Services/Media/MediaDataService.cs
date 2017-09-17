namespace Scrapper.Domain.Services.Media
{
    using System.Threading.Tasks;
    using Scrapper.Data.Repository.Interfaces;
    using Scrapper.Domain.Core.Interfaces.Media;
    using Scrapper.Domain.Model.Media;

    public class MediaDataService : IMediaDataService
    {
        private readonly IMediaRepository mediaRepository;

        public MediaDataService(
            IMediaRepository mediaRepository)
        {
            this.mediaRepository = mediaRepository;
        }

        public async Task<Media> InsertMediaAsync(Media media)
        {
            return await this.mediaRepository.AddAsync(media).ConfigureAwait(false);
        }
    }
}
