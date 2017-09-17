namespace Scrapper.Data.Repository.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MongoDB.Driver;
    using Scrapper.Data.Repository.Interfaces;

    public abstract class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        private readonly IMongoCollection<T> collection;

        public MongoRepository(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            this.collection = database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> matchExpression)
        {
            return await collection.Find(matchExpression).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> matchExpression)
        {
            return await collection.Find(matchExpression).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await collection.Find(Builders<T>.Filter.Empty).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entityList)
        {
            await collection.InsertManyAsync(entityList);
            return entityList;
        }
    }
}
