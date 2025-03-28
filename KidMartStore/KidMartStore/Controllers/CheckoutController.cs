using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using Stripe;
using Stripe.Checkout;
using System.Globalization;
using KidMartStore.Models;
using System.Data.Entity;
using KidMartStore.Patterns.Singleton;
using KidMartStore.Patterns.Adapter;

namespace KidMartStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly KidMartStoreEntities database = DatabaseContextSingleton.Instance;

        private IPaymentAdapter GetPaymentAdapter(string paymentMethod)
        {
            if (paymentMethod == "Stripe") return new StripeAdapter();
            if (paymentMethod == "PayPal") return new PayPalAdapter();
            throw new Exception("Unsupported payment method.");
        }

        [HttpPost]
        public JsonResult CreateCheckoutSession(string paymentMethod)
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || cart.Items.Count() == 0)
            {
                return Json(new { error = "Cart is empty" }, JsonRequestBehavior.AllowGet);
            }

            var adapter = GetPaymentAdapter(paymentMethod);
            var domain = Request.Url.GetLeftPart(UriPartial.Authority);
            var successUrl = $"{domain}/Checkout/PaymentSuccess?method={paymentMethod}&session_id={{CHECKOUT_SESSION_ID}}";
            var cancelUrl = $"{domain}/Home/GioHang";

            //Lấy adapter
            string sessionId = adapter.CreatePaymentSession(cart, successUrl, cancelUrl);

            return Json(new { sessionId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentSuccess(string method, string session_id)
        {
            var adapter = GetPaymentAdapter(method);

            if (adapter.ProcessPayment(session_id))
            {
                return ProcessOrderAfterPayment();
            }

            return RedirectToAction("GioHang", "Home");
        }
        private ActionResult ProcessOrderAfterPayment()
        {
            try
            {
                int? MaKH = Session["ID_Customer"] as int?;
                if (MaKH == null)
                {
                    return Content("Customer not logged in.");
                }

                Cart cart = Session["Cart"] as Cart;
                if (cart == null || cart.Items.Count() == 0)
                {
                    return Content("Your cart is empty.");
                }

                using (var transaction = database.Database.BeginTransaction())
                {
                    try
                    {
                        // Create new order
                        Models.Order _order = new Models.Order
                        {
                            Order_Date = DateTime.Now,
                            ID_Customer = (int)MaKH,
                            Total = Convert.ToDecimal(cart.TotalMoney()),
                            Status = "Đã thanh toán" // "Paid"
                        };
                        database.Orders.Add(_order);
                        database.SaveChanges(); // Save to generate ID_Order

                        foreach (var item in cart.Items)
                        {
                            Detail_Order _order_detail = new Detail_Order
                            {
                                ID_Order = _order.ID_Order,
                                ID_Product = item._product.ID_Product,
                                Unit_Price = Convert.ToDecimal(item._product.Price),
                                Quantity = item._quantity
                            };
                            database.Detail_Order.Add(_order_detail);

                            var product = database.Products.SingleOrDefault(p => p.ID_Product == item._product.ID_Product);
                            if (product != null)
                            {
                                product.Quantity = Math.Max(0, product.Quantity - item._quantity);
                            }
                        }

                        database.SaveChanges();
                        transaction.Commit();

                        cart.ClearCart();
                        Session["Cart"] = null;
                        return RedirectToAction("CheckOut_Success", "Product");
                    }
                    catch
                    {
                        transaction.Rollback();
                        return Content("Transaction failed. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("Checkout failed: " + ex.Message);
            }
        }
    }
}