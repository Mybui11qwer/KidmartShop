using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Controllers.Class
{
    public interface IAddressService
    {
        string GetCustomerAddress(long customerId);
    }

    public class RealAddressService : IAddressService
    {
        private readonly KidMartStoreEntities db;

        public RealAddressService(KidMartStoreEntities dbContext)
        {
            db = dbContext;
        }

        public string GetCustomerAddress(long customerId)
        {
            var customer = db.Customers.Find(customerId);
            return customer?.Address ?? "Chưa cập nhật địa chỉ";
        }
    }

    public class ProxyAddressService : IAddressService
    {
        private RealAddressService realAddressService;
        private Dictionary<long, string> addressCache = new Dictionary<long, string>();

        public ProxyAddressService(KidMartStoreEntities dbContext)
        {
            realAddressService = new RealAddressService(dbContext);
        }

        public string GetCustomerAddress(long customerId)
        {
            if (!addressCache.ContainsKey(customerId))
            {
                addressCache[customerId] = realAddressService.GetCustomerAddress(customerId);
            }
            return addressCache[customerId];
        }
    }
}