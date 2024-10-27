using System.Linq;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    public KidMartStoreEntities db = new KidMartStoreEntities();

    public ActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Login(Customer KhachHang)
    {
        var checkEmail = db.Customers.Where(s => s.Email == KhachHang.Email).FirstOrDefault();
        var checkPassword = db.Customers.Where(s => s.Password == KhachHang.Password).FirstOrDefault();
        if (checkEmail == null || checkPassword == null)
        {
            if (checkEmail == null || checkPassword == null)
            {
                ModelState.AddModelError("", "Tài Khoản Không Tồn Tại");
                return View("Login");
            }
        }
        else
        {
            Session["Email"] = KhachHang.Email;
            Session["FullName"] = KhachHang.Phone;
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(Customer KhachHangMoi)
    {
        try
        {
            db.Customers.Add(KhachHangMoi);
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
