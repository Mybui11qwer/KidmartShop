using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Data.Entity;

namespace KidMartStore.Areas.Admin.Controllers
{
    // Command Pattern - Interface cho các lệnh
    public interface IOrderCommand
    {
        bool Execute();
    }

    // Concrete Command - Cập nhật trạng thái đơn hàng
    public class UpdateOrderStatusCommand : IOrderCommand
    {
        private readonly KidMartStoreEntities _database;
        private readonly int _orderId;
        private readonly string _status;
        private readonly IOrderObserver[] _observers;

        public UpdateOrderStatusCommand(KidMartStoreEntities database, int orderId, string status, params IOrderObserver[] observers)
        {
            _database = database;
            _orderId = orderId;
            _status = status;
            _observers = observers;
        }

        public bool Execute()
        {
            try
            {
                var order = _database.Orders.Find(_orderId);
                if (order != null)
                {
                    string oldStatus = order.Status;
                    order.Status = _status;
                    _database.SaveChanges();

                    // Thông báo cho tất cả các observer
                    foreach (var observer in _observers)
                    {
                        observer.OrderUpdated(order, oldStatus, _status);
                    }

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    // Observer Pattern - Interface cho các observer
    public interface IOrderObserver
    {
        void OrderUpdated(Order order, string oldStatus, string newStatus);
    }

    // Concrete Observer - Ghi log khi đơn hàng được cập nhật
    public class OrderLogObserver : IOrderObserver
    {
        public void OrderUpdated(Order order, string oldStatus, string newStatus)
        {
            // Giả lập việc ghi log
            System.Diagnostics.Debug.WriteLine($"Đơn hàng #{order.ID_Order} đã được cập nhật từ '{oldStatus}' sang '{newStatus}' lúc {DateTime.Now}");
        }
    }

    public class SettingOrderController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();

        // Observer 
        private readonly IOrderObserver _logObserver = new OrderLogObserver();

        // GET: Admin/SettingOrder
        public ActionResult Index()
        {
            var orders = database.Orders.Include(o => o.Customer).Include(o => o.Detail_Order.Select(d => d.Product))
                .OrderByDescending(o => o.Order_Date).ToList();
            return View(orders);
        }

        // GET: Admin/SettingOrder/Details/5
        public ActionResult Details(int id)
        {
            var order = database.Orders.Include(o => o.Customer)
                .Include(o => o.Detail_Order.Select(d => d.Product))
                .FirstOrDefault(o => o.ID_Order == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // POST: Admin/SettingOrder/UpdateStatus
        [HttpPost]
        public ActionResult UpdateOrderStatus(int orderId, string status)
        {
            var order = database.Orders.Find(orderId);
            if (order != null && order.Status != "Đã giao")
            {
                if (status == "Hủy đơn" || status == "Yêu cầu hoàn tiền")
                {
                    var orderDetails = database.Detail_Order.Where(od => od.ID_Order == orderId).ToList();
                    foreach (var item in orderDetails)
                    {
                        var product = database.Products.Find(item.ID_Product);
                        if (product != null)
                        {
                            product.Quantity += item.Quantity; // Hoàn trả số lượng sản phẩm
                        }
                    }
                }
                order.Status = status;
                database.SaveChanges();
                return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
            }
            return Json(new { success = false, message = "Đơn hàng không tồn tại!" });
        }
    }
}