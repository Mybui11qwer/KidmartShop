using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidMartStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly KidMartStoreEntities database = new KidMartStoreEntities();
        // GET: User
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
        public ActionResult GioHanng()
        {
            return View();
        }
    }
}