using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Controllers
{
    public static class CustomerFactory
    {
        public static Customer CreateCustomer(string name, string email, string password)
        {
            return new Customer
            {
                Username = name,
                Email = email,
                Password = password,
                Role = "Khách Hàng"
            };
        }
    }
}