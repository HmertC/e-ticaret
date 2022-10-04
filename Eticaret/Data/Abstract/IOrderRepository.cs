using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
