namespace Scrapper.Data.Repository.Mongo
{
    using Scrapper.Data.Repository.Interfaces;
    using Scrapper.Domain.Model.Media;

    public class MediaRepository : MongoRepository<Media>, IMediaRepository
    {
        public MediaRepository(string connectionString, string databaseName)
            :base(connectionString, databaseName)
        {
        }
    }
}
