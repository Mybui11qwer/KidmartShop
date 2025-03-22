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
        private readonly ProductService _productService;
        private readonly IAddressService _addressService = new ProxyAddressService(new KidMartStoreEntities());

        public HomeController()
        {
            _productService = new ProductService(db);
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
        // Áp dụng mẫu Proxy
        public ActionResult Address()
        {
            var customerId = Convert.ToInt64(Session["ID_Customer"]);
            ViewBag.Address = _addressService.GetCustomerAddress(customerId);
            return View();
        }

        public ActionResult GioHang()
        {
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
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