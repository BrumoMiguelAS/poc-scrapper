namespace Scrapper.Data.Repository.Interfaces
{
    using Scrapper.Domain.Model.Scrapping.Sources;

    public interface IScrappingSourceRepository : IMongoRepository<ScrappingSource>
    {
    }
}
