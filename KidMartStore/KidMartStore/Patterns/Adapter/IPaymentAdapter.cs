using KidMartStore.Models;
using Stripe.Checkout;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PayPal.Api;
using System.Globalization;

namespace KidMartStore.Patterns.Adapter
{
    public interface IPaymentAdapter
    {
        string CreatePaymentSession(Cart cart, string successUrl, string cancelUrl);
        bool ProcessPayment(string sessionId);
    }

    public class StripeAdapter : IPaymentAdapter
    {
        public string CreatePaymentSession(Cart cart, string successUrl, string cancelUrl)
        {
            string stripeSecretKey = System.Configuration.ConfigurationManager.AppSettings["StripeSecretKey"];
            StripeConfiguration.ApiKey = stripeSecretKey;

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
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.Id;
        }

        public bool ProcessPayment(string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);
            return session.PaymentStatus == "paid";
        }
    }

    public class PayPalAdapter : IPaymentAdapter
    {
        public string CreatePaymentSession(Cart cart, string successUrl, string cancelUrl)
        {
            var clientId = System.Configuration.ConfigurationManager.AppSettings["PayPalClientID"];
            var secret = System.Configuration.ConfigurationManager.AppSettings["PayPalSecret"];
            var mode = System.Configuration.ConfigurationManager.AppSettings["PayPalMode"];

            var config = new Dictionary<string, string> { { "mode", mode } };
            var accessToken = new OAuthTokenCredential(clientId, secret, config).GetAccessToken();
            var apiContext = new APIContext(accessToken) { Config = config };

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
                        total = cart.TotalMoney().ToString("F2", CultureInfo.InvariantCulture)
                    },
                    description = "Purchase from KidMartStore"
                }
            },
                redirect_urls = new RedirectUrls
                {
                    return_url = successUrl,
                    cancel_url = cancelUrl
                }
            };

            var createdPayment = payment.Create(apiContext);
            return createdPayment.links.FirstOrDefault(l => l.rel == "approval_url")?.href;
        }

        public bool ProcessPayment(string sessionId)
        {
            return true; // PayPal xử lý theo URL redirect, không cần xác nhận sessionId
        }
    }
}
