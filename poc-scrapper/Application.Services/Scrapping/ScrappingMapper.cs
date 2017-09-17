namespace Scrapper.Application.Services.Scrapping
{
    using Scrapper.Application.Dto.Scrapping.Sources;
    using DomainScrapping = Domain.Model.Scrapping.Sources;

    public static class ScrappingMapper
    {
        public static ScrappingSource ScrappingSourceToDto(DomainScrapping.ScrappingSource source)
        {
            return new ScrappingSource
            {
                Id = source.Id.ToString(),
                Name = source.Name,
                PaginationPattern = source.PaginationPattern,
                Url = source.Url
            };
        }

        public static DomainScrapping.ScrappingSource ScrappingSourceToModel(ScrappingSource source)
        {
            return new DomainScrapping.ScrappingSource
            {
                Name = source.Name,
                PaginationPattern = source.PaginationPattern,
                Url = source.Url.ToLowerInvariant()
            };
        }
    }
}
