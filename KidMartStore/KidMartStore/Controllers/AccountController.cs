using System.Linq;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private KidMartStoreEntities db = new KidMartStoreEntities();

    public ActionResult Login()
    {
        return View();
    }
    public ActionResult Register()
    {
        return View();
    }
    // Xử lý Đăng ký
    [HttpPost]
    public ActionResult Register(string Username, string Password, string Email)
    {
        if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Email))
        {
            // Kiểm tra xem email đã tồn tại chưa
            var existingUser = db.Khách_Hàng.FirstOrDefault(u => u.Email == Email);
            if (existingUser == null)
            {
                // Mã hóa mật khẩu trước khi lưu
                string hashedPassword = HashPassword(Password);

                // Tạo người dùng mới
                Khách_Hàng newUser = new Khách_Hàng
                {
                    Họ_và_Tên = Username,
                    Password = hashedPassword,
                    Email = Email
                };

                db.Khách_Hàng.Add(newUser);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", "Username already exists.");
            }
        }
        return View();
    }

    // Xử lý Đăng nhập
    [HttpPost]
    public ActionResult Login(string Email, string Password)
    {
        if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
        {
            var user = db.Khách_Hàng.FirstOrDefault(u => u.Email == Email);
            if (user != null && VerifyPassword(Password, user.Password))
            {
                // Lưu session khi đăng nhập thành công
                Session["UserId"] = user.Mã_KH;
                Session["Username"] = user.Họ_và_Tên;

                return RedirectToAction("Index", "Home");
            }
        }
        return View();
    }

    // Đăng xuất
    public ActionResult Logout()
    {
        Session.Clear(); // Xóa tất cả session khi đăng xuất
        return RedirectToAction("Login");
    }

    // Hàm mã hóa mật khẩu
    private string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    // Hàm kiểm tra mật khẩu
    private bool VerifyPassword(string enteredPassword, string storedPassword)
    {
        string hashedEnteredPassword = HashPassword(enteredPassword);
        return hashedEnteredPassword == storedPassword;
    }
}
