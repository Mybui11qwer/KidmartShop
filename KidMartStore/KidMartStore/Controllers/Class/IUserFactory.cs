using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidMartStore.Controllers.Class
{
    public interface IUserFactory
    {
        Customer CreateUser(string email, string password, string username, string address, string phone);
    }
    public class CustomerFactory : IUserFactory
    {
        public Customer CreateUser(string email, string password, string username, string address, string phone)
        {
            return new Customer
            {
                Email = email,
                Password = password,
                Username = username,
                Address = address,
                Phone = phone,
                Role = "Khách Hàng"
            };
        }
    }
}
