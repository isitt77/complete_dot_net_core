using System;
using System.Linq;
using System.Linq.Expressions;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CompleteDotNetCore.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            //_db.Products.Include(u => u.Category);
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {   // Lets us pass in as many Include properties as we need.
                foreach (string includeProperty in includeProperties
                    .Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.ToList();
        }

        public T? GetFirstOrDefault(Expression<Func<T, bool>> filter,
            string? includeProperties = null, bool tracked = true)
        {
            IQueryable<T> query;

            // Boolean, tracked, to determine whether EF Core tracks
            // changes in database. (See OrderController
            // UpdateOrderDetails(). )
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            if (includeProperties != null)
            {   // Lets us pass in as many Include properties as we need.
                foreach (string includeProperty in includeProperties
                    .Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}

