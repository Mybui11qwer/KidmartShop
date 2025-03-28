using KidMartStore.Models;
using System.Collections.Generic;

namespace KidMartStore.Areas.Admin.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        int GetTotalProducts();
    }
} 