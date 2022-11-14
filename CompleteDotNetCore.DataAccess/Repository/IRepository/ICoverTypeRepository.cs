using System;
using CompleteDotNetCore.Models;

namespace CompleteDotNetCore.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType obj);
    }
}

