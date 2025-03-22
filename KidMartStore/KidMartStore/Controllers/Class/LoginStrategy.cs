using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidMartStore.Controllers.Class
{
    internal interface LoginStrategy
    {
        Customer Authenticate(string identifier, string password);
    }
    //Login with email.
    public class EmailLoginStrategy : LoginStrategy
    {
        private readonly KidMartStoreEntities _db;

        public EmailLoginStrategy(KidMartStoreEntities db)
        {
            _db = db;
        }

        public Customer Authenticate(string email, string password)
        {
            return _db.Customers.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
    //Login with phone .num
    public class PhoneLoginStrategy : LoginStrategy
    {
        private readonly KidMartStoreEntities _db;

        public PhoneLoginStrategy(KidMartStoreEntities db)
        {
            _db = db;
        }

        public Customer Authenticate(string phone, string password)
        {
            return _db.Customers.FirstOrDefault(u => u.Phone == phone && u.Password == password);
        }
    }
}
