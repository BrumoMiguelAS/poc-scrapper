namespace Scrapper.Domain.Model.Scrapping.Sources
{
    using MongoDB.Bson;

    public class ScrappingSource
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string PaginationPattern { get; set; }
    }
}
