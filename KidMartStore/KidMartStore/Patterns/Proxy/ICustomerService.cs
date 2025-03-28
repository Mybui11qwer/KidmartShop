using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Patterns.Proxy
{
    public interface ICustomerService
    {
        Customer GetCustomerProfile(long customerId);
        string GetCustomerAddress(long customerId);
    }


    public class RealCustomerService : ICustomerService
    {
        private readonly KidMartStoreEntities _db = new KidMartStoreEntities();

        public Customer GetCustomerProfile(long customerId)
        {
            return _db.Customers.Find(customerId);
        }

        public string GetCustomerAddress(long customerId)
        {
            var customer = _db.Customers.Find(customerId);
            return customer?.Address ?? "Không có địa chỉ";
        }
    }


    public class ProxyCustomerService : ICustomerService
    {
        private readonly RealCustomerService _realService = new RealCustomerService();

        public Customer GetCustomerProfile(long customerId)
        {
            if (customerId <= 0)
            {
                throw new UnauthorizedAccessException("Bạn chưa đăng nhập!");
            }
            return _realService.GetCustomerProfile(customerId);
        }

        public string GetCustomerAddress(long customerId)
        {
            if (customerId <= 0)
            {
                throw new UnauthorizedAccessException("Bạn chưa đăng nhập!");
            }
            return _realService.GetCustomerAddress(customerId);
        }
    }

}