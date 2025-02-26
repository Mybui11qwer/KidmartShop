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
            var MaKH = Session["ID_Customer"];
            var customer = database.Customers.Find(MaKH);
            return View(customer);
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
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        public ActionResult CancelOrder(int id)
        {
            var order = database.Orders.FirstOrDefault(o => o.ID_Order == id);
            if (order != null && order.Status != "Đã giao")
            {
                order.Status = "Đã hủy";
                database.SaveChanges();
                TempData["Message"] = "Đơn hàng đã được hủy thành công!";
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn hàng này!";
            }
            return RedirectToAction("ShowOrder");
        }
    }
}