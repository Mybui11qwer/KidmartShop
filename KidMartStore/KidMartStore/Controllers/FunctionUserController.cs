﻿using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidMartStore.Controllers
{
    public class FunctionUserController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();

        //Hủy đơn hàng.
        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            var order = database.Orders.FirstOrDefault(o => o.ID_Order == id);

            if (order != null && order.Status != "Đã giao")
            {
                // Lấy danh sách sản phẩm trong đơn hàng
                var orderDetails = database.Detail_Order.Where(od => od.ID_Order == id).ToList();

                foreach (var item in orderDetails)
                {
                    var product = database.Products.FirstOrDefault(p => p.ID_Product == item.ID_Product);
                    if (product != null)
                    {
                        product.Quantity += item.Quantity;
                        database.SaveChanges();// Cộng lại số lượng sản phẩm vào kho
                    }
                }

                // Cập nhật trạng thái đơn hàng
                order.Status = "Đã hủy";
                database.SaveChanges();

                TempData["Message"] = "Đơn hàng đã được hủy thành công! Số lượng sản phẩm đã được cập nhật.";
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn hàng này!";
            }
            return RedirectToAction("ShowOrder", "Home");
        }
    }
}