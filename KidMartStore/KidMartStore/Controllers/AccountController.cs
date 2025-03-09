using System.Linq;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.PeerToPeer;
using System.Web.UI.WebControls;
using KidMartStore.Controllers;

public class AccountController : Controller
{
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Login(Customer customer)
    {
        if (ModelState.IsValid)
        {
            // Sử dụng Singleton thay vì tạo mới đối tượng DbContext
            var db = DbContextSingleton.Instance;

            var checkUser = db.Customers.FirstOrDefault(u => u.Email == customer.Email && u.Password == customer.Password);
            if (checkUser != null)
            {
                // Lưu thông tin vào session
                Session["Email"] = checkUser.Email;
                Session["Name"] = checkUser.Username;
                Session["Address"] = checkUser.Address;
                Session["Phone"] = checkUser.Phone;
                Session["ID_Customer"] = checkUser.ID_Customer;
                Session["Role"] = checkUser.Role;

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email hoặc password sai, vui lòng thử lại!";
        }
        return View();
    }

    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(Customer newCustomer)
    {
        var db = DbContextSingleton.Instance; // Sử dụng Singleton
        var existingCustomer = db.Customers.FirstOrDefault(c => c.Email == newCustomer.Email);
        if (existingCustomer != null)
        {
            ViewBag.Error = "Email đã tồn tại";
            return View();
        }

        try
        {
            //Sử dụng Factory
            var customer = CustomerFactory.CreateCustomer(newCustomer.Username, newCustomer.Email, newCustomer.Password);
            db.Customers.Add(customer);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
        catch
        {
            return View("Register");
        }
    }

    // Đăng xuất
    public ActionResult Logout()
    {
        Session.Clear(); // Xóa tất cả session khi đăng xuất
        return RedirectToAction("Login");
    }
}
