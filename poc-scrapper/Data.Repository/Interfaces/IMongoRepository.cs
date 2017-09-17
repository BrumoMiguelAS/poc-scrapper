namespace Scrapper.Data.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IMongoRepository<T> where T : class
    {
        Task<T> FindAsync(Expression<Func<T, bool>> matchExpression);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> matchExpression);

        Task<IEnumerable<T>> FindAllAsync();

        Task<T> AddAsync(T entity);

        Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> entityList);
    }
}
