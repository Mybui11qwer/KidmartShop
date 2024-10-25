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
    [HttpPost]
    public ActionResult Login(Login model)
    {
        if (ModelState.IsValid)
        {
            // Here, you would authenticate the user with the database

            bool isValidUser = AuthenticateUser(model.Email, model.Password); // Define AuthenticateUser
            if (isValidUser)
            {
                TempData["Message"] = "Login successful!";
                // Redirect to the user's dashboard or homepage
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid email or password");
            }
        }

        return View(model);
    }
    public ActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Register(Register model)
    {
        if (ModelState.IsValid)
        {
            // Here, you would add the user to the database
            // and possibly send a confirmation email

            TempData["Message"] = "Registration successful!";
            return RedirectToAction("Login");
        }

        return View(model);
    }

    private bool AuthenticateUser(string email, string password)
    {
        // You should query the database to validate the user credentials.
        // For demo purposes, we'll just return false.
        return false;
    }

    // Đăng xuất
    public ActionResult Logout()
    {
        Session.Clear(); // Xóa tất cả session khi đăng xuất
        return RedirectToAction("Login");
    }
}
