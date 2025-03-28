using KidMartStore.Models;
using System.Collections.Generic;

namespace KidMartStore.Areas.Admin.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        int GetTotalOrders();
        IEnumerable<Order> GetOrdersWithDetails();
    }
} 