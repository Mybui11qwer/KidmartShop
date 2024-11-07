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
            // Lấy tổng số người dùng
            var totalUsers = database.Customers.Count(c => c.Role == "Khách Hàng");
            // Lấy tổng số nhân viên
            var totalAdmins = database.Customers.Count(c => c.Role == "Nhân Viên" || c.Role == "Quản Lý");

            // Lấy tổng số sản phẩm
            var totalProducts = database.Products.Count();

            // Lấy tổng số đơn hàng
            var totalOrders = database.Orders.Count();

            // Truyền dữ liệu qua ViewBag
            ViewBag.TotalAdmins = totalAdmins;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalOrders = totalOrders;

            return View();
        }
        public ActionResult ManagerAccount()
        {
            List<Customer> customers = database.Customers.ToList();
            return View(customers);
        }
        public ActionResult ManagerAccountAdmin()
        {
            List<Customer> customers = database.Customers.ToList();
            return View(customers);
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
                return RedirectToAction("ManagerAccount");
            }
            catch
            {
                return View("AddNewAccount");
            }
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
                if (NewProduct.UploadImage != null && NewProduct.UploadImage.ContentLength > 0)
                {
                    try
                    {
                        string directoryPath = Server.MapPath("~/Image/");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string filename = Path.GetFileNameWithoutExtension(NewProduct.UploadImage.FileName);
                        string extension = Path.GetExtension(NewProduct.UploadImage.FileName);
                        filename = filename + "_" + Guid.NewGuid().ToString() + extension;
                        string path = Path.Combine(directoryPath, filename);

                        // Lưu đường dẫn vào thuộc tính Image
                        NewProduct.Image = "~/Image/" + filename;  
                        NewProduct.UploadImage.SaveAs(path);
                    }
                    catch (Exception ex)
                    {
                        // Ghi log lỗi hoặc xử lý lỗi
                        Console.WriteLine("Lỗi khi lưu tệp: " + ex.Message);
                    }
                }

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
                    existingProduct.ID_Category = product.ID_Category;
                    existingProduct.Quantity = product.Quantity;
                    existingProduct.Image = product.Image; // Only update if a new image is uploaded

                    database.SaveChanges(); // Save changes to the database
                }

                return RedirectToAction("ManagerProduct"); // Redirect back to the product manager
            } // Redirect to the product list
            return View(product);
        }
        public ActionResult ManagerCategory()
        {
            List<Category> categories = database.Categories.ToList();
            return View(categories);
        }
        public ActionResult AddNewCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewCategory(Category NewCategory)
        {
            try
            {
                database.Categories.Add(NewCategory);
                database.SaveChanges();
                return RedirectToAction("ManagerCategory");
            }
            catch
            {
                return View("AddNewCategory");
            }
        }
    }
}