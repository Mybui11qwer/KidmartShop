using KidMartStore.Models;
using System.Linq;

namespace KidMartStore.Areas.Admin.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(KidMartStoreEntities context) : base(context)
        {
        }

        public int GetTotalProducts()
        {
            return _dbSet?.Count() ?? 0;
        }
    }
} 