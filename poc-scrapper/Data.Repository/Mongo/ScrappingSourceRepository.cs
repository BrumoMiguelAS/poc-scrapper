namespace Scrapper.Data.Repository.Mongo
{
    using Scrapper.Data.Repository.Interfaces;
    using Scrapper.Domain.Model.Scrapping.Sources;

    public class ScrappingSourceRepository : MongoRepository<ScrappingSource>, IScrappingSourceRepository
    {
        public ScrappingSourceRepository(string connectionString, string databaseName)
            :base(connectionString, databaseName)
        {
        }
    }
}
