using System.Linq;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.PeerToPeer;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System;

namespace KidMartStore.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();
        //Thêm
        public ActionResult AddNewAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewAccount(Customer NewCustomer)
        {
            try
            {
                database.Customers.Add(NewCustomer);
                database.SaveChanges();
                return RedirectToAction("ManagerAccount", "Home", new { area = "Admin" });
            }
            catch
            {
                return View("AddNewAccount");
            }
        }
        //Sửa
        public ActionResult UpdateAccount(int id)
        {
            var customer = database.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        public ActionResult UpdateAccount(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = database.Customers.Find(customer.ID_Customer);
                if (existingCustomer != null)
                {
                    existingCustomer.Username = customer.Username;
                    existingCustomer.Phone = customer.Phone;
                    existingCustomer.Email = customer.Email;
                    existingCustomer.Address = customer.Address;
                    existingCustomer.Gender = customer.Gender;

                    database.SaveChanges();
                }
                return RedirectToAction("ManagerAccount", "Home", new { area = "Admin" });
            }
            return View(customer);
        }
        //Xóa
        public ActionResult DeleteAccount(int id)
        {
            var customer = database.Customers.Find(id);
            if (customer != null)
            {
                database.Customers.Remove(customer);
                database.SaveChanges();
            }
            return RedirectToAction("ManagerAccount", "Home", new { area = "Admin" });
        }
    }
}

