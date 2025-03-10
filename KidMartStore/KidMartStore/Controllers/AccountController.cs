using System.Linq;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.PeerToPeer;
using System.Web.UI.WebControls;

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
                Session["Role"] = checkUser.Role;

                return RedirectToAction("Index", "Home");
            }
            if (checkUser == null)
            {
                ViewBag.Error = "Email hoặc password sai, vui lòng thử lại!";
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
        if(NewCustomer.Password.ToString().Length <= 9)
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
