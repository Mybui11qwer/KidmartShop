using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KidMartStore.Models;

namespace KidMartStore.Areas.Admin.Controllers
{
    [Route("Admin/Home")]
    public class HomeController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();
        // GET: Admin/Home
        public ActionResult Index()
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
    }
}