using System.Linq;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.PeerToPeer;

public class AccountController : Controller
{
    public KidMartStoreEntities db = new KidMartStoreEntities();

    public ActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Login(Customer customer)
    {
        if (ModelState.IsValid)
        {
            // Kiểm tra thông tin đăng nhập
            var checkUser = db.Customers.FirstOrDefault(u => u.Email == customer.Email && u.Password == customer.Password);
            if (checkUser != null)
            {
                // Lưu tên người dùng vào session
                Session["Email"] = checkUser.Email;
                Session["Name"] = checkUser.Username;
                Session["Address"] = checkUser.Address;
                Session["Phone"] = checkUser.Phone;
                Session["ID_Customer"] = checkUser.ID_Customer;

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
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
        try
        {
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
