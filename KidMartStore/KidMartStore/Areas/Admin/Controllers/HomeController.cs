using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Data.Entity;
using KidMartStore.Areas.Admin.Repositories;

namespace KidMartStore.Areas.Admin.Controllers
{
    [Route("Admin/Home")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private bool _disposed = false;

        public HomeController()
        {
            _unitOfWork = new UnitOfWork(new KidMartStoreEntities());
        }

        // For testing with dependency injection
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/Home
        public ActionResult Index()
        {
            // Lấy tổng số người dùng
            var totalUsers = _unitOfWork.Customers.GetTotalUsers();

            // Lấy tổng số nhân viên
            var totalAdmins = _unitOfWork.Customers.GetTotalAdmins();

            // Lấy tổng số sản phẩm
            var totalProducts = _unitOfWork.Products.GetTotalProducts();

            // Lấy tổng số đơn hàng
            var totalOrders = _unitOfWork.Orders.GetTotalOrders();

            // Truyền dữ liệu qua ViewBag
            ViewBag.TotalAdmins = totalAdmins;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalOrders = totalOrders;

            return View();
        }

        public ActionResult ManagerAccount()
        {
            List<Customer> customers = _unitOfWork.Customers.GetAll().ToList();
            return View(customers);
        }

        public ActionResult ManagerProduct()
        {
            List<Product> products = _unitOfWork.Products.GetAll().ToList();
            return View(products);
        }

        public ActionResult ManagerOrders()
        {
            var orders = _unitOfWork.Orders.GetOrdersWithDetails();
            return View(orders);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}