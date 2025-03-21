using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWebKidMart.Controllers
{
    public class FunctionController : Controller
    {
        private readonly KidMartStoreEntities database = new KidMartStoreEntities();
        public ActionResult AddNewProduct()
        {
            List<Category> categories = database.Categories.ToList();
            return View(categories);
        }
        [HttpPost]
        public ActionResult AddNewProduct(Product NewProduct)
        {
            try
            {
                if (NewProduct.UploadImage != null && NewProduct.UploadImage.ContentLength > 0)
                {
                    try
                    {
                        string newFileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(NewProduct.UploadImage.FileName);
                        string NewName = newFileName + extension;
                        string FileName = Path.GetFileName(NewName);
                        string path = Path.Combine(Server.MapPath("~/Image/Product/"), FileName);

                        string externalFolder = @"D:\ShopBanQuanAo\KidMartStore\KidMartStore\Image\Product\";
                        string externalPath = Path.Combine(externalFolder, FileName);
                        // Lưu đường dẫn vào thuộc tính Image
                        NewProduct.Image = FileName;
                        NewProduct.UploadImage.SaveAs(path);
                        NewProduct.UploadImage.SaveAs(externalPath);
                    }
                    catch (Exception ex)
                    {
                        // Ghi log lỗi hoặc xử lý lỗi
                        Console.WriteLine("Lỗi khi lưu tệp: " + ex.Message);
                    }
                }

                database.Products.Add(NewProduct);
                database.SaveChanges();
                return RedirectToAction("ManagerProduct", "Admin");
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
        public ActionResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {

                // Update the product in the database
                var existingProduct = database.Products.Find(product.ID_Product);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Quantity = product.Quantity;

                    database.SaveChanges(); // Save changes to the database
                }
            } // Redirect to the product list
            return RedirectToAction("ManagerProduct", "Admin");
        }

        public ActionResult UpdateAccount(int id)
        {
            var customer = database.Products.Find(id); // Adjust this line based on your data access method
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        public ActionResult UpdateAccount(Customer customer)
        {
            if (ModelState.IsValid)
            {

                // Update the customer in the database
                var existingCustomer = database.Customers.Find(customer.ID_Customer);
                if (existingCustomer != null)
                {
                    existingCustomer.Username = customer.Username;
                    existingCustomer.Phone = customer.Phone;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Address = customer.Address;
                    existingCustomer.Gender = customer.Gender;

                    database.SaveChanges(); // Save changes to the database
                }

                return RedirectToAction("ManagerAccount", "Admin"); // Redirect back to the customer manager
            } // Redirect to the customer list
            return View(customer);
        }

        public ActionResult AddNewAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewAccount(Customer NewCustomer)
        {
            try
            {
                database.Customers.Add(NewCustomer);
                database.SaveChanges();
                return RedirectToAction("ManagerAccount", "Admin");
            }
            catch
            {
                return View("AddNewAccount");
            }
        }
        public ActionResult DeleteAccount(int id)
        {
            var customer = database.Customers.Find(id);
            if (customer != null)
            {
                database.Customers.Remove(customer);
                database.SaveChanges();
            }
            return RedirectToAction("ManagerAccount", "Admin"); // Hoặc trang danh sách người dùng.
        }
    }
}