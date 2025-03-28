using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidMartStore.Areas.Admin.Controllers
{
    public class SettingOrderController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();
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
                            product.Quantity += item.Quantity;
                            database.SaveChanges();
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