using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Controllers.Class
{
    public class ProductService
    {
        private readonly KidMartStoreEntities _db;

        public ProductService(KidMartStoreEntities db)
        {
            _db = db;
        }

        // Lấy danh sách sản phẩm với bộ lọc
        public List<Product> GetFilteredProducts(string category, string query, string filter, int page, int pageSize, out int totalProducts)
        {
            IProductFactory factory = ProductFactory.CreateFactory(filter);
            List<Product> products = factory.GetProducts(_db);

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name_Category == category).ToList();
            }

            // Tìm kiếm sản phẩm theo từ khóa
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.Name.Contains(query) || p.Description.Contains(query)).ToList();
            }

            // Tổng số sản phẩm trước khi phân trang
            totalProducts = products.Count();

            // Phân trang
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        // Lấy danh sách slider
        public List<Slider> GetActiveSliders()
        {
            return _db.Sliders.Where(s => s.Active).OrderBy(s => s.Position).ToList();
        }

        // Lấy chi tiết sản phẩm theo ID
        public Product GetProductById(int id)
        {
            return _db.Products.FirstOrDefault(p => p.ID_Product == id);
        }

        // Lấy danh sách sản phẩm có phân trang
        public List<Product> GetPagedProducts(int page, int pageSize, out int totalProducts)
        {
            totalProducts = _db.Products.Count();
            return _db.Products.OrderBy(p => p.ID_Product) // Sắp xếp để đảm bảo dữ liệu ổn định
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
        }
    }
}