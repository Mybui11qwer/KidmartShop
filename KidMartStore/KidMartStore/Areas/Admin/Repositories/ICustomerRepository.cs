using KidMartStore.Models;
using System.Collections.Generic;

namespace KidMartStore.Areas.Admin.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        int GetTotalUsers();
        int GetTotalAdmins();
    }
} 