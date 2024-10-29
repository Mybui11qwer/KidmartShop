using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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
            try {
                database.Products.Add(NewProduct);
                database.SaveChanges();
                return RedirectToAction("ManagerProduct");
            }
            catch
            {
                return View("AddNewProduct");
            }
        }
        public ActionResult UpdateProduct(int id)
        {
            var product = database.Products.Find(id); // Adjust this line based on your data access method
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpPost]
        public ActionResult UpdateProduct(Product product, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload if a new image is provided
                if (Image != null && Image.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Image.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    Image.SaveAs(path);
                    product.Image = "~/Content/Images/" + fileName; // Update the model with new image path
                }

                // Update the product in the database
                var existingProduct = database.Products.Find(product.ID_Product);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.ID_Category = product.ID_Category;
                    existingProduct.Quantity = product.Quantity;
                    existingProduct.Image = product.Image; // Only update if a new image is uploaded

                    database.SaveChanges(); // Save changes to the database
                }

                return RedirectToAction("ManagerProduct"); // Redirect back to the product manager
            } // Redirect to the product list
            return View(product);
        }
    }
}