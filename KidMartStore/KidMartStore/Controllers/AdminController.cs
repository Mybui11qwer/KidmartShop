using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using KidMartStore.Models;

namespace KidMartStore.Controllers
{
    public class AdminController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            // Lấy tổng số người dùng (nếu không có, trả về 0)
            var totalUsers = database.Customers?.Count(c => c.Role == "Khách Hàng") ?? 0;

            // Lấy tổng số nhân viên (nếu không có, trả về 0)
            var totalAdmins = database.Customers?.Count(c => c.Role == "Nhân Viên" || c.Role == "Quản Lý") ?? 0;

            // Lấy tổng số sản phẩm (nếu không có, trả về 0)
            var totalProducts = database.Products?.Count() ?? 0;

            // Lấy tổng số đơn hàng (nếu không có, trả về 0)
            var totalOrders = database.Orders?.Count() ?? 0;

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

        public ActionResult ManagerProduct()
        {
            List<Product> products = database.Products.ToList();
            return View(products);
        }
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
                    existingProduct.Quantity = product.Quantity;

                    database.SaveChanges(); // Save changes to the database
                }
            } // Redirect to the product list
            return RedirectToAction("ManagerProduct");
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