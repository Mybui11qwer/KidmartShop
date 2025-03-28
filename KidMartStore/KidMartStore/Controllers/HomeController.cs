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
using KidMartStore.Patterns.Singleton;
using KidMartStore.Patterns.Factory;
using KidMartStore.Patterns.Proxy;
using KidMartStore.Patterns.Builder;

namespace KidMartStore.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        //Áp dụng singleon để connect database
        private readonly KidMartStoreEntities database = DatabaseContextSingleton.Instance;

        //Áp dụng factory product
        private readonly ProductService _productService;


        //Áp dụng proxy vào để lấy thông tin khách hàng
        private readonly ICustomerService _customerService = new ProxyCustomerService();

        public HomeController()
        {
            _productService = new ProductService(database);
        }

        // Áp dụng Facade và Factory
        public ActionResult Index(string category, string query, string filter, int page = 1)
        {
            int pageSize = 4;
            int totalProducts;
            var products = _productService.GetFilteredProducts(category, query, filter, page, pageSize, out totalProducts);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.Category = category;
            ViewBag.Query = query;
            ViewBag.Sliders = _productService.GetActiveSliders();

            return View(products);
        }

        public ActionResult GioiThieu()
        {
            return View();
        }
        // Áp dụng Facade và Factory
        public ActionResult SanPham(int? page)
        {
            int pageSize = 16;
            int pageNumber = page ?? 1;
            int totalProducts;

            var products = _productService.GetPagedProducts(pageNumber, pageSize, out totalProducts);

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(products);
        }
        public new ActionResult Profile()
        {
            var MaKH = Convert.ToInt64(Session["ID_Customer"]);
            var customer = _customerService.GetCustomerProfile(MaKH);
            return View(customer);
        }

        [HttpPost]
        public new ActionResult Profile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var userId = Convert.ToInt64(Session["ID_Customer"]);
                var user = _customerService.GetCustomerProfile(userId);

                if (user != null)
                {
                    user.Username = customer.Username;
                    user.Address = customer.Address;
                    user.Phone = customer.Phone;

                    using (var db = new KidMartStoreEntities())
                    {
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    Session["Email"] = user.Email;
                    Session["Name"] = user.Username;
                    Session["Phone"] = user.Phone;
                    Session["Address"] = user.Address;

                    TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
                    return RedirectToAction("Profile");
                }
            }
            return View(customer);
        }

        public ActionResult Address()
        {
            var customerId = Convert.ToInt64(Session["ID_Customer"]);
            ViewBag.Address = _customerService.GetCustomerAddress(customerId);
            return View();
        }

        public ActionResult GioHang()
        {
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        //Áp dụng Facade
        public ActionResult ChiTietSanPham(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
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
                var ordersData = database.Orders
                    .Where(d => d.ID_Customer == customerID)
                    .Include(o => o.Detail_Order.Select(p => p.Product))
                    .ToList();

                // Sử dụng OrderBuilder để tạo danh sách đơn hàng
                var orders = ordersData.Select(o =>
                    new OrderBuilder()
                        .SetCustomerId(o.ID_Customer)
                        .AddDetailOrder(o.Detail_Order.ToList())
                        .SetStatus(o.Status)
                        .Build()
                ).ToList();

                return View(orders);
            }
            return RedirectToAction("Login", "Auth");
        }
        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
    }
}