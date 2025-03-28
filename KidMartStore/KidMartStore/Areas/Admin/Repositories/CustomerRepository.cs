using KidMartStore.Models;
using System.Linq;

namespace KidMartStore.Areas.Admin.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(KidMartStoreEntities context) : base(context)
        {
        }

        public int GetTotalUsers()
        {
            return _dbSet?.Count(c => c.Role == "Khách Hàng") ?? 0;
        }

        public int GetTotalAdmins()
        {
            return _dbSet?.Count(c => c.Role == "Nhân Viên" || c.Role == "Quản Lý") ?? 0;
        }
    }
} 