using System;
using CompleteDotNetCore.Models;

namespace CompleteDotNetCore.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}

