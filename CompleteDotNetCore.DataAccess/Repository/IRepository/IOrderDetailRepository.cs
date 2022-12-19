using System;
using CompleteDotNetCore.Models;

namespace CompleteDotNetCore.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}

