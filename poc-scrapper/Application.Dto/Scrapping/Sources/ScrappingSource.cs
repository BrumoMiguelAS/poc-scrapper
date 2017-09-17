namespace Scrapper.Application.Dto.Scrapping.Sources
{
    public class ScrappingSource
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string PaginationPattern { get; set; }
    }
}
