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
<<<<<<< HEAD
        {
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
=======
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

        public ActionResult UpdateAccount(int id)
        {
            var product = database.Products.Find(id); // Adjust this line based on your data access method
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
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

                return RedirectToAction("ManagerAccount"); // Redirect back to the customer manager
            } // Redirect to the customer list
            return View(customer);
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
>>>>>>> Laptop-My
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
<<<<<<< HEAD
=======
        public ActionResult DeleteAccount(int id)
        {
            var customer = database.Customers.Find(id);
            if (customer != null)
            {
                database.Customers.Remove(customer);
                database.SaveChanges();
            }
            return RedirectToAction("ManagerAccount"); // Hoặc trang danh sách người dùng.
        }

        public ActionResult DeleteAccountAdmin(int id)
        {
            var customer = database.Customers.Find(id);
            if (customer != null)
            {
                database.Customers.Remove(customer);
                database.SaveChanges();
            }
            return RedirectToAction("ManagerAccountAdmin"); // Hoặc trang danh sách người dùng.
        }

>>>>>>> Laptop-My
        public ActionResult ManagerProduct()
        {
            List<Product> products = database.Products.ToList();
            return View(products);
        }
        public ActionResult AddNewProduct()
        {
<<<<<<< HEAD
            return View();
        }
        [HttpPost]
        public ActionResult AddNewProduct(Product NewProduct)
        {
            try {
=======
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

                        // Lưu đường dẫn vào thuộc tính Image
                        NewProduct.Image = FileName;  
                        NewProduct.UploadImage.SaveAs(path);
                    }
                    catch (Exception ex)
                    {
                        // Ghi log lỗi hoặc xử lý lỗi
                        Console.WriteLine("Lỗi khi lưu tệp: " + ex.Message);
                    }
                }

>>>>>>> Laptop-My
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
<<<<<<< HEAD
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
=======
        public ActionResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
>>>>>>> Laptop-My

                // Update the product in the database
                var existingProduct = database.Products.Find(product.ID_Product);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
<<<<<<< HEAD
                    existingProduct.ID_Category = product.ID_Category;
                    existingProduct.Quantity = product.Quantity;
                    existingProduct.Image = product.Image; // Only update if a new image is uploaded

                    database.SaveChanges(); // Save changes to the database
                }

                return RedirectToAction("ManagerProduct"); // Redirect back to the product manager
            } // Redirect to the product list
            return View(product);
=======
                    existingProduct.Quantity = product.Quantity;

                    database.SaveChanges(); // Save changes to the database
                }
            } // Redirect to the product list
            return RedirectToAction("ManagerProduct");
>>>>>>> Laptop-My
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