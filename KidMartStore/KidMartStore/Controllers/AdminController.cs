using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KidMartStore.Models;

namespace KidMartStore.Controllers
{
    public class AdminController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult ManagerAccount()
        {
            List<Customer> customers = database.Customers.ToList();
            return View(customers);
        }
        public ActionResult AddNewAccount()
        {
            return View();
        }
        public ActionResult ManagerProduct()
        {
            List<Product> products = database.Products.ToList();
            return View(products);
        }
        public ActionResult AddNewProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewProduct(Product NewProduct)
        {
            try
            {
                database.Products.Add(NewProduct);
                database.SaveChanges();
                return RedirectToAction("ManagerProduct");
            }
            catch
            {
                return View("AddNewProduct");
            }
        }
    }
}