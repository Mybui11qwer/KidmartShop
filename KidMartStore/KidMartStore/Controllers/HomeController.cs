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

namespace KidMartStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly KidMartStoreEntities database = new KidMartStoreEntities();
        public ActionResult Index()
        {
            List<Product> products = database.Products.ToList();
            return View(products);
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult SanPham()
        {
            List<Product> products = database.Products.ToList();
            return View(products);
        }
        public ActionResult Profile()
        {
<<<<<<< HEAD
            return View();
=======
            var MaKH = Session["ID_Customer"];
            var customer = database.Customers.Find(MaKH);
            return View(customer);
>>>>>>> Laptop-My
        }
        [HttpPost]
        public ActionResult Profile(Customer customer)
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
<<<<<<< HEAD
=======
            return View();
        }
        public ActionResult ShowOrder()
        {
            if (Session["ID_Customer"]!= null)
            {
                var customerID = Convert.ToInt64(Session["ID_Customer"]);
                var orders = database.Detail_Order
                    .Include(d => d.Order) // Include để truy cập bảng liên quan Order // Include để truy cập bảng liên quan Product
                    .Where(d => d.Order.ID_Customer == customerID) // Lọc theo ID_Customer trong Order
                    .ToList();
                return View(orders);
            }
            if (Session["ID_Customer"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
>>>>>>> Laptop-My
            return View();
        }
    }
}