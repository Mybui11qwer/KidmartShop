using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.Helpers;
using System.Globalization;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;
using System.Runtime.Remoting.Contexts;
using System.Web.UI;
using KidMartStore.Controllers.Class;

namespace KidMartStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly KidMartStoreEntities db = DatabaseContextSingleton.Instance;
        public ActionResult Index(string category, string query, int page = 1, string filter = "Default")
        {
            int pageSize = 4;

            IProductFactory factory = ProductFactory.CreateFactory(filter);
            List<Product> products = factory.GetProducts(db);

            // Lọc theo danh mục nếu có
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name_Category == category).ToList();
            }

            // Tìm kiếm sản phẩm nếu có từ khóa query
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.Name.Contains(query) || p.Description.Contains(query)).ToList();
            }

            int totalProducts = products.Count(); // Tổng số sản phẩm
            products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.Category = category;
            ViewBag.Query = query;

            var sliders =   db.Sliders.Where(s => s.Active).OrderBy(s => s.Position).ToList();
            ViewBag.Sliders = sliders;

            return View(products);
        }

        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult SanPham(int? page)
        {
            int pageSize = 16; // Số sản phẩm mỗi trang
            int pageNumber = page ?? 1; // Nếu không có page, mặc định là 1

            List<Product> products = db.Products.ToList();
            int totalProducts = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            // Kiểm tra giới hạn của pageNumber
            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages) pageNumber = totalPages;

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;

            products = products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View(products);
        }
        public new ActionResult Profile()
        {
            var MaKH = Session["ID_Customer"];
            var customer = db.Customers.Find(MaKH);
            return View(customer);
        }
        [HttpPost]
        public new ActionResult Profile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var userId = Convert.ToInt64(Session["ID_Customer"]); // Retrieve the current user ID
                var user = db.Customers.Find(userId);

                if (user != null)
                {
                    user.Username = customer.Username;
                    user.Address = customer.Address;
                    user.Phone = customer.Phone;
                    // Update other properties

                    db.SaveChanges();

                    Session["Email"] = user.Email;
                    Session["Name"] = user.Username;
                    Session["Phone"] = user.Phone;
                    Session["Address"] = user.Address;

                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction("Profile"); // Adjust as needed
                }
            }
            return View(customer);
        }

        public ActionResult Address()
        {
            return View();
        }

        public ActionResult GioHang()
        {
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        public ActionResult ChiTietSanPham(int id)
        {
            // Retrieve the product from the database based on the product ID
            var product = db.Products.FirstOrDefault(p => p.ID_Product == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Pass product data to the view
            return View(product);
        }
        public ActionResult CheckOut_Success(string paymentMethod)
        {
            ViewBag.PaymentMethod = paymentMethod;
            return View();
        }
        public ActionResult ShowOrder()
        {
            if (Session["ID_Customer"] != null)
            {
                var customerID = Convert.ToInt64(Session["ID_Customer"]);
                var orders = db.Orders
                    .Where(d => d.ID_Customer == customerID)
                    .Include(o => o.Detail_Order.Select(p => p.Product))
                    .ToList();
                return View(orders);
            }
            if (Session["ID_Customer"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}