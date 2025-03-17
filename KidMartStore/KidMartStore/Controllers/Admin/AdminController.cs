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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Bỏ qua kiểm tra cho các trang Login và Logout
            if (filterContext.ActionDescriptor.ActionName != "Login" &&
                filterContext.ActionDescriptor.ActionName != "Logout")
            {
                if (Session["Email"] == null)
                {
                    filterContext.Result = RedirectToAction("Login", "Admin");
                }
            }
            base.OnActionExecuting(filterContext);
        }


        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra thông tin đăng nhập
                var checkUser = database.Customers.FirstOrDefault(u => u.Email == customer.Email && u.Password == customer.Password);
                if (checkUser != null)
                {
                    // Lưu tên người dùng vào session
                    Session["Email"] = checkUser.Email;
                    Session["Name"] = checkUser.Username;
                    Session["Address"] = checkUser.Address;
                    Session["Phone"] = checkUser.Phone;
                    Session["ID_Customer"] = checkUser.ID_Customer;
                    Session["Role"] = checkUser.Role;
                    if (checkUser.Role == "Khách Hàng")
                    {
                        ViewBag.Error = "Đây là tài khoản khách hàng";
                        return View();
                    }
                    if (checkUser.Role != "Khách Hàng")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                }
                else
                {
                    // Kiểm tra nếu email không tồn tại
                    var user = database.Customers.FirstOrDefault(u => u.Email == customer.Email);
                    if (user == null)
                    {
                        ViewBag.ErrorEmail = "Email không tồn tại";
                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorPassword = "Sai mật khẩu";
                        return View();
                    }
                }
            }
            return View();
        }
        // Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa tất cả session khi đăng xuất
            return RedirectToAction("Login");
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

        public ActionResult ManagerAccountAdmin()
        {
            List<Customer> customers = database.Customers.ToList();
            return View(customers);
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
        
        public ActionResult ManagerCategory()
        {
            List<Category> categories = database.Categories.ToList();
            return View(categories);
        }
    }
}