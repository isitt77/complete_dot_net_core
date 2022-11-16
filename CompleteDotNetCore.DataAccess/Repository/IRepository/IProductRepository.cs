using System;
using CompleteDotNetCore.Models;

namespace CompleteDotNetCore.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}

