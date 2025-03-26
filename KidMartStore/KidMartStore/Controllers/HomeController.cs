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

namespace KidMartStore.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly KidMartStoreEntities database = new KidMartStoreEntities();
        public ActionResult Index(string category, string query, int page = 1)
        {
            int pageSize = 4;
            List<Product> products = database.Products.ToList();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name_Category == category).ToList();
            }

            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.Name.Contains(query) || p.Description.Contains(query)).ToList();
            }

            int totalProducts = products.Count();
            products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.Category = category;
            ViewBag.Query = query;

            var sliders = database.Sliders.Where(s => s.Active).OrderBy(s => s.Position).ToList();
            ViewBag.Sliders = sliders;

            return View(products);
        }

        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult SanPham(int? page)
        {
            int pageSize = 16; 
            int pageNumber = page ?? 1; 

            List<Product> products = database.Products.ToList();
            int totalProducts = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

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
            var customer = database.Customers.Find(MaKH);
            return View(customer);
        }
        [HttpPost]
        public new ActionResult Profile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var userId = Convert.ToInt64(Session["ID_Customer"]); // Retrieve the current user ID
                var user = database.Customers.Find(userId);

                if (user != null)
                {
                    user.Username = customer.Username;
                    user.Address = customer.Address;
                    user.Phone = customer.Phone;
                    // Update other properties

                    database.SaveChanges();

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
            var product = database.Products.FirstOrDefault(p => p.ID_Product == id);
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
                var orders = database.Orders
                    .Where(d => d.ID_Customer == customerID)
                    .Include(o => o.Detail_Order.Select(p => p.Product))
                    .ToList();
                return View(orders);
            }
            if (Session["ID_Customer"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
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