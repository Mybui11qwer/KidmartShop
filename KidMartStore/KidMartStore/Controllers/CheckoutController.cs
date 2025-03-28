﻿using System;
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

namespace KidMartStore.Controllers
{
    public class CheckoutController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();

        [HttpPost]
        public JsonResult CreateCheckoutSession()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || cart.Items.Count() == 0)
            {
                return Json(new { error = "Cart is empty" }, JsonRequestBehavior.AllowGet);
            }

            bool isTestMode = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["StripeTestMode"]);
            string stripeSecretKey = isTestMode
                ? System.Configuration.ConfigurationManager.AppSettings["StripeTestSecretKey"]
                : System.Configuration.ConfigurationManager.AppSettings["StripeLiveSecretKey"];

            StripeConfiguration.ApiKey = stripeSecretKey; // This ensures the correct key is used

            var domain = Request.Url.GetLeftPart(UriPartial.Authority);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cart.Items.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(item._product.Price * 100), // Convert to cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item._product.Name
                        }
                    },
                    Quantity = item._quantity
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{domain}/Checkout/StripeSuccess?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/Home/GioHang"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { sessionId = session.Id }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StripeSuccess(string session_id)
        {
            var service = new SessionService();
            var session = service.Get(session_id);

            if (session.PaymentStatus == "paid")
            {
                return ProcessOrderAfterPayment();
            }

            return RedirectToAction("GioHanng", "Product");
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




        public ActionResult PayWithPayPal()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null || cart.Items.Count() == 0)
            {
                return RedirectToAction("GioHanng", "Product");
            }

            var clientId = System.Configuration.ConfigurationManager.AppSettings["PayPalClientID"];
            var secret = System.Configuration.ConfigurationManager.AppSettings["PayPalSecret"];
            var mode = System.Configuration.ConfigurationManager.AppSettings["PayPalMode"];

            var config = new Dictionary<string, string>
    {
        { "mode", mode }
    };

            var accessToken = new OAuthTokenCredential(clientId, secret, config).GetAccessToken();
            var apiContext = new APIContext(accessToken) { Config = config };

            // Create PayPal transaction
            var totalAmount = cart.TotalMoney().ToString("F2", CultureInfo.InvariantCulture);
            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
        {
            new Transaction
            {
                amount = new Amount
                {
                    currency = "USD",
                    total = totalAmount
                },
                description = "Purchase from KidMartStore"
            }
        },
                redirect_urls = new RedirectUrls
                {
                    return_url = Url.Action("PayPalSuccess", "Product", null, Request.Url.Scheme),
                    cancel_url = Url.Action("GioHanng", "Product", null, Request.Url.Scheme)
                }
            };

            var createdPayment = payment.Create(apiContext);

            // Get approval URL from PayPal
            var approvalUrl = createdPayment.links.FirstOrDefault(l => l.rel == "approval_url")?.href;

            return Redirect(approvalUrl);
        }

        public ActionResult PayPalSuccess(string paymentId, string PayerID)
        {
            try
            {
                var clientId = System.Configuration.ConfigurationManager.AppSettings["PayPalClientID"];
                var secret = System.Configuration.ConfigurationManager.AppSettings["PayPalSecret"];
                var mode = System.Configuration.ConfigurationManager.AppSettings["PayPalMode"];

                var config = new Dictionary<string, string> { { "mode", mode } };
                var accessToken = new OAuthTokenCredential(clientId, secret, config).GetAccessToken();
                var apiContext = new APIContext(accessToken) { Config = config };

                var paymentExecution = new PaymentExecution { payer_id = PayerID };
                var payment = new Payment { id = paymentId };
                var executedPayment = payment.Execute(apiContext, paymentExecution);

                if (executedPayment.state.ToLower() != "approved")
                {
                    return Content("Payment was not successful.");
                }

                // Get customer email from PayPal transaction
                var customerEmail = executedPayment.payer.payer_info.email;
                var customer = database.Customers.FirstOrDefault(c => c.Email == customerEmail);
                if (customer == null)
                {
                    return Content("Customer not found.");
                }

                var cart = Session["Cart"] as Cart;
                if (cart == null || cart.Items.Count() == 0)
                {
                    return Content("Cart is empty.");
                }

                // Save order details
                Models.Order _order = new Models.Order
                {
                    Order_Date = DateTime.Now,
                    ID_Customer = customer.ID_Customer,
                    Total = (int)cart.TotalMoney(),
                    Status = "Paid"
                };
                database.Orders.Add(_order);
                database.SaveChanges();

                foreach (var item in cart.Items)
                {
                    Detail_Order _order_detail = new Detail_Order
                    {
                        ID_Order = _order.ID_Order,
                        ID_Product = item._product.ID_Product,
                        Unit_Price = (int)item._product.Price,
                        Quantity = item._quantity
                    };
                    database.Detail_Order.Add(_order_detail);

                    // Update stock quantity
                    var product = database.Products.SingleOrDefault(p => p.ID_Product == item._product.ID_Product);
                    if (product != null)
                    {
                        product.Quantity -= item._quantity;
                        if (product.Quantity < 0) product.Quantity = 0;
                    }
                }
                database.SaveChanges();

                // Clear cart
                cart.ClearCart();
                Session["Cart"] = null;

                return RedirectToAction("CheckOut_Success", new { orderId = _order.ID_Order });
            }
            catch (Exception ex)
            {
                return Content("Error processing PayPal payment: " + ex.Message);
            }
        }

    }
}