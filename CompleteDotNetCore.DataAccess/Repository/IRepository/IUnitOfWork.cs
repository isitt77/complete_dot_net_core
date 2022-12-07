using System;
namespace CompleteDotNetCore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IProductRepository Product { get; }
        IProductRepository Company { get; }
        void Save();
    }
}

