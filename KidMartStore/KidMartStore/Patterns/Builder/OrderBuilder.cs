using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Patterns.Builder
{
    public class OrderBuilder
    {
        private Order _order;

        public OrderBuilder()
        {
            _order = new Order();
        }

        public OrderBuilder SetCustomerId(int customerId)
        {
            _order.ID_Customer = customerId;
            return this;
        }

        public OrderBuilder AddDetailOrder(List<Detail_Order> details)
        {
            _order.Detail_Order = details;
            return this;
        }

        public OrderBuilder SetStatus(string status)
        {
            _order.Status = status;
            return this;
        }

        public Order Build()
        {
            return _order;
        }
    }
}