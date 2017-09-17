namespace Scrapper.Applciation.Services.Media
{
    using System;
    using System.Threading.Tasks;
    using Scrapper.Application.Dto.Media;
    using Scrapper.Application.Services.Media;
    using Scrapper.Domain.Core.Interfaces.Media;
    using DomainMedia = Domain.Model.Media;

    public class MediaService : IMediaService
    {
        private readonly IMediaDataService mediaService;

        public MediaService(
            IMediaDataService mediaService)
        {
            this.mediaService = mediaService;
        }

        public async Task<Media> InsertMediaAsync(Media media)
        {
            if (media == null)
            {
                throw new ArgumentException("Invalid media");
            }

            var newMedia = MediaMapper.MediaToDomain(media);

            var insertedMedia = await this.mediaService.InsertMediaAsync(newMedia).ConfigureAwait(false);

            return media;
        }
    }
}
