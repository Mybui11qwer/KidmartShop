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
    private readonly KidMartStoreEntities db;
    private readonly IUserFactory _userFactory;
    private readonly LoginStrategy _loginStrategy;

    public AccountController(LoginStrategy loginStrategy, KidMartStoreEntities _db, IUserFactory userFactory)
    {
        db = _db;
        _userFactory = userFactory;
        _loginStrategy = loginStrategy;
    }

    public ActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Login(string identifier, string password)
    {
        var user = _loginStrategy.Authenticate(identifier, password);
        if (user != null)
        {
            Session["Email"] = user.Email;
            Session["Name"] = user.Username;
            return RedirectToAction("Index", "Home");
        }
        ViewBag.ErrorEmail = user == null ? "Email không tồn tại" : null;
        ViewBag.ErrorPassword = user != null ? "Sai mật khẩu" : null;
        return View();
    }

    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(string email, string password, string username, string address, string phone)
    {
        var existingCustomer = db.Customers.FirstOrDefault(c => c.Email == email);
        if (existingCustomer != null)
        {
            ViewBag.Error = "Email đã tồn tại";
            return View();
        }
        if (password.Length <= 9)
        {
            ViewBag.Error1 = "Mật khẩu phải dài hơn 9 ký tự";
            return View();
        }

        var newCustomer = _userFactory.CreateUser(email, password, username, address, phone);
        db.Customers.Add(newCustomer);
        db.SaveChanges();
        return RedirectToAction("Login");
    }

    public ActionResult Logout()
    {
        Session.Clear();
        return RedirectToAction("Login");
    }
}

