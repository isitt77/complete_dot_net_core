using System;
using System.Linq.Expressions;

namespace CompleteDotNetCore.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // Find()
        T? GetFirstOrDefault(Expression<Func<T, bool>> filter,
            string? includeProperties = null, bool tracked = true);

        // GET classes
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null);

        // Add()
        void Add(T entity);

        // Remove() <-- One
        void Remove(T entity);

        // Remove() <-- Many
        void RemoveRange(IEnumerable<T> entity);
    }
}

