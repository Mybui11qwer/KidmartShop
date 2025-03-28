using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KidMartStore.Controllers.Class
{
    // Interface phải có phương thức GetProducts()
    public interface IProductFactory
    {
        List<Product> GetProducts(KidMartStoreEntities db);
    }

    // Factory chính tạo các loại Factory khác
    public static class ProductFactory
    {
        public static IProductFactory CreateFactory(string type)
        {
            switch (type)
            {
                case "Available":
                    return new AvailableProductFactory(); // Phải đảm bảo lớp này tồn tại
                default:
                    return new DefaultProductFactory();
            }
        }
    }

    // Factory mặc định - lấy tất cả sản phẩm
    public class DefaultProductFactory : IProductFactory
    {
        public List<Product> GetProducts(KidMartStoreEntities db)
        {
            return db.Products.ToList();
        }
    }

    // Factory lấy sản phẩm còn hàng
    public class AvailableProductFactory : IProductFactory
    {
        public List<Product> GetProducts(KidMartStoreEntities db)
        {
            return db.Products.Where(p => p.Quantity > 0).ToList();
        }
    }
}