using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.Mvc;
using KidMartStore.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.Helpers;

namespace KidMartStore.Controllers
{
    public class HomeController : Controller
    {
        private KidMartStoreEntities database = new KidMartStoreEntities();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var userCheck = database.Khách_Hàng.Where(x => x.Email.Equals(Email) && x.Password.Equals(Password)).ToList();
                if (userCheck.Count() > 0)
                {
                    // Lưu session khi đăng nhập thành công
                    Session["UserId"] = userCheck.FirstOrDefault().Mã_KH;
                    Session["Username"] = userCheck.FirstOrDefault().Họ_và_Tên;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(string NameParent, string Email, string Password)
        {
            if (!string.IsNullOrEmpty(NameParent) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Email))
            {
                // Kiểm tra xem username đã tồn tại chưa
                var existingUser = database.Khách_Hàng.FirstOrDefault(u => u.Email == Email);
                if (existingUser == null)
                {
                    // Mã hóa mật khẩu trước khi lưu
                    string hashedPassword = HashPassword(Password);

                    // Tạo người dùng mới
                    Khách_Hàng newUser = new Khách_Hàng
                    {
                        Họ_và_Tên = NameParent,
                        Password = hashedPassword,
                        Email = Email
                    };
                    database.Khách_Hàng.Add(newUser);
                    database.SaveChanges();

                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Username already exists.");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa tất cả session khi đăng xuất
            return RedirectToAction("Login");
        }

        public ActionResult Index(int? page)
        {
            int? pageSize = 10;
            int? pageNumber = page == null || page < 0 ? 1 : page.Value;
            return View();
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
}