using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidMartStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly KidMartStoreEntities database = new KidMartStoreEntities();
        // GET: User
        public ActionResult ChiTietSanPham(int id)
        {
            // Retrieve the product from the database based on the product ID
            var product = database.Products.FirstOrDefault(p => p.ID_Product == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Pass product data to the view
            return View(product);
        }
        public ActionResult GioHanng()
        {
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if(cart == null | Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int id)
        {
            var product = database.Products.SingleOrDefault(s => s.ID_Product == id);
            if(product != null)
            {
                GetCart().Add_Product_Cart(product);
            }
            return RedirectToAction("Index", "Home");
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
                database.Orders.Add(_order);

                foreach (var item in cart.Items)
                {
                    Detail_Order _order_detail = new Detail_Order(); // Lưu dòng sản phẩm vào bảng Chi Tiết Hóa Đơn
                    _order_detail.ID_Order = _order.ID_Order;
                    _order_detail.ID_Product = item._product.ID_Product;
                    _order_detail.Unit_Price = (int)item._product.Price;
                    _order_detail.Quantity = item._quantity;
                    database.Detail_Order.Add(_order_detail);
                }
                database.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("CheckOut_Success", "Product");
            }
            catch
            {
                return Content("Error checkout. Please check information of Customer...Thanks.");
            }
        }
        public ActionResult CheckOut_Success()
        {
            return View();
        }

    }
}