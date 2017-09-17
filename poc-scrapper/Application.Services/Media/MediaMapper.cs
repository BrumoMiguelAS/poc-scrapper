namespace Scrapper.Application.Services.Media
{
    using System.Linq;
    using Application.Dto.Media;
    using DomainMedia = Domain.Model.Media;

    public static class MediaMapper
    {
        public static Media MediaToDto(DomainMedia.Media media)
        {
            return new Media
            {
                Id = media.Id.ToString(),
                Name = media.Name,
                OriginalUrl = media.OriginalUrl,
                Type = (MediaType)(int)media.Type,
                PhysicalPath = media.PhysicalPath,
                Status = (MediaStatus)(int)media.Status,
                Properties = media.Properties.Select(MediaPropertiesToDto).ToList(),
            };
        }

        public static DomainMedia.Media MediaToDomain(Media media)
        {
            return new DomainMedia.Media
            {
                Name = media.Name,
                OriginalUrl = media.OriginalUrl,
                PhysicalPath = media.PhysicalPath,
                Type = (DomainMedia.MediaType)(int)media.Type,
                Status = (DomainMedia.MediaStatus)(int)media.Status,
                Properties = media.Properties.Select(MediaPropertiesToModel).ToList(),
            };
        }

        private static MediaProperties MediaPropertiesToDto(DomainMedia.MediaProperties mediaProps)
        {
            return new MediaProperties
            {
                Score = mediaProps.Score,
                Value = mediaProps.Value,
                Type = (MediaPropertyType)(int)mediaProps.Type,
            };
        }

        private static DomainMedia.MediaProperties MediaPropertiesToModel(MediaProperties mediaProps)
        {
            return new DomainMedia.MediaProperties
            {
                Score = mediaProps.Score,
                Value = mediaProps.Value,
                Type = (DomainMedia.MediaPropertyType)(int)mediaProps.Type,
            };
        }
    }
}
