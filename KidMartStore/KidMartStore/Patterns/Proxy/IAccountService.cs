using KidMartStore.Models;
using KidMartStore.Patterns.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Patterns.Proxy
{
    internal interface IAccountService
    {
        Customer Login(string email, string password);
    }
    public class AccountService : IAccountService
    {
        private readonly KidMartStoreEntities db = DatabaseContextSingleton.Instance;

        public Customer Login(string email, string password)
        {
            return db.Customers.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
    public class AccountServiceProxy : IAccountService
    {
        private readonly AccountService _accountService = new AccountService();

        public Customer Login(string email, string password)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Email và mật khẩu không được để trống.");
            }

            // Gọi dịch vụ thực tế
            var customer = _accountService.Login(email, password);

            if (customer == null)
            {
                Console.WriteLine("Đăng nhập thất bại: Sai email hoặc mật khẩu.");
            }
            else
            {
                Console.WriteLine("Đăng nhập thành công: " + customer.Username);
            }

            return customer;
        }
    }
}