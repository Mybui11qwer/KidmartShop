﻿using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidMartStore.Controllers
{
    public class FunctionCartController : Controller
    {
        private readonly KidMartStoreEntities database = new KidMartStoreEntities();
        // GET: User
        

        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null | Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            if (Session["ID_Customer"] != null)
            {
                var product = database.Products.SingleOrDefault(s => s.ID_Product == id);
                if (product != null)
                {
                    GetCart().Add_Product_Cart(product, quantity);
                }
                return RedirectToAction("GioHang", "Home");
            }
            if (Session["ID_Customer"] == null)
            {

                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public JsonResult Update_Cart_Quantity(int id, int cartQuantity)
        {
            var cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                cart.Update_quantity(id, cartQuantity); // Cập nhật số lượng
                var total = cart.TotalMoney(); // Lấy tổng tiền mới
                return Json(new { success = true, totalMoney = total }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Giỏ hàng không tồn tại" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            if (cart.Items.Count() == 0)
            {
                // Xóa Session nếu giỏ hàng không còn sản phẩm
                Session["Cart"] = null; 
            }
            return RedirectToAction("GioHang", "Home");
        }
        public ActionResult CheckOut()
        {
            try
            {
                var MaKH = Session["ID_Customer"];
                Cart cart = Session["Cart"] as Cart;
                Order _order = new Order(); // Bảng Hóa Đơn Sản Phẩm
                _order.Order_Date = DateTime.Now;
                _order.ID_Customer = (int)MaKH;
                _order.Total = (int)cart.TotalMoney();
                _order.Status = "Chờ xác nhận";
                _order.PaymentMethod = "cod";
                database.Orders.Add(_order);

                foreach (var item in cart.Items)
                {
                    Detail_Order _order_detail = new Detail_Order(); // Lưu dòng sản phẩm vào bảng Chi Tiết Hóa Đơn
                    _order_detail.ID_Order = _order.ID_Order;
                    _order_detail.ID_Product = item._product.ID_Product;
                    _order_detail.Unit_Price = (int)item._product.Price;
                    _order_detail.Quantity = item._quantity;
                    _order_detail.ID_Size = 1;
                    database.Detail_Order.Add(_order_detail);

                    // Giảm số lượng sản phẩm trong kho
                    var product = database.Products.SingleOrDefault(p => p.ID_Product == item._product.ID_Product);
                    if (product != null)
                    {
                        product.Quantity -= item._quantity;
                        if (product.Quantity < 0) // Kiểm tra nếu hết hàng
                        {
                            product.Quantity = 0;
                        }
                    }
                }
                database.SaveChanges();
                cart.ClearCart();
                Session["Cart"] = null;
                return RedirectToAction("CheckOut_Success", "Home");
            }
            catch
            {
                return Content("Error checkout. Please check information of Customer...Thanks.");
            }
        }
        [HttpPost]
        public JsonResult ApplyVoucher(string code)
        {
            decimal discount = 0;

            if (code == "SALE10") discount = 0.1m;
            else if (code == "SALE20") discount = 0.2m;

            var cart = (Cart)Session["Cart"];
            decimal newTotal = cart.TotalMoney() * (1 - discount);

            return Json(new { success = true, totalMoney = newTotal });
        }
    }
}