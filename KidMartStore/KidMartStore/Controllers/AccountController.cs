using System.Linq;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.PeerToPeer;
using System.Web.UI.WebControls;
using KidMartStore.Controllers.Class;

public class AccountController : Controller
{
    private readonly KidMartStoreEntities db = DatabaseContextSingleton.Instance;
    private readonly IAccountService _accountService = new AccountServiceProxy();
    public ActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Login(Customer customer)
    {
        if (ModelState.IsValid)
        {
            //Áp dụng proxy
            var checkUser = _accountService.Login(customer.Email, customer.Password);

            if (checkUser != null)
            {
                Session["Email"] = checkUser.Email;
                Session["Name"] = checkUser.Username;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                return View();
            }
        }
        return View();
    }
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(Customer NewCustomer)
    {
        var existingCustomer = db.Customers.FirstOrDefault(c => c.Email == NewCustomer.Email);
        if (existingCustomer != null)
        {
            ViewBag.Error = "Email đã tồn tại";
            return View();
        }
        if (NewCustomer.Password.ToString().Length <= 9)
        {
            ViewBag.Error1 = "Mật khẩu phải dài hơn 9 ký tự";
            return View();
        }
        try
        {
            NewCustomer.Role = "Khách Hàng";
            db.Customers.Add(NewCustomer);
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