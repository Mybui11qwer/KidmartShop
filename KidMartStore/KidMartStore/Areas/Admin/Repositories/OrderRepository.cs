using KidMartStore.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace KidMartStore.Areas.Admin.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(KidMartStoreEntities context) : base(context)
        {
        }

        public int GetTotalOrders()
        {
            return _dbSet?.Count() ?? 0;
        }

        public IEnumerable<Order> GetOrdersWithDetails()
        {
            return _dbSet.Include(o => o.Detail_Order.Select(d => d.Product))
                .OrderByDescending(o => o.Order_Date).ToList();
        }
    }
} 