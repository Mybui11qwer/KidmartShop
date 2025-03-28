using System;

namespace KidMartStore.Areas.Admin.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        
        void Save();
    }
} 