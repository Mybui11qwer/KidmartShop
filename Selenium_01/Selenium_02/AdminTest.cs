using Microsoft.Office.Interop.Excel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Timers;
using Excel = Microsoft.Office.Interop.Excel;

namespace Selenium_01
{
    [TestFixture]
    public class AdminTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        Excel.Application dataApp;
        Excel.Workbook workBook;
        Excel.Worksheet dataSheet;
        Excel.Range xlRange, xlRange2;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://localhost:44369/Admin");
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //Actions action = new Actions(driver);

            //Mở Excel
            dataApp = new Excel.Application();
            if (System.IO.File.Exists("C:\\Users\\mybui\\OneDrive - Ho Chi Minh City University of Foreign Languages and Information Technology - HUFLIT\\TestCase Nhom3\\Nhom 3.xlsx"))
            {
                workBook = dataApp.Workbooks.Open(@"C:\Users\mybui\OneDrive - Ho Chi Minh City University of Foreign Languages and Information Technology - HUFLIT\TestCase Nhom3\Nhom 3.xlsx");
                Console.Write("Hello");
            }
            else
            {
                Console.WriteLine("File không tồn tại!");
            }
            dataSheet = (Excel.Worksheet)workBook.Sheets[3];
            xlRange = dataSheet.UsedRange;

            dataSheet = (Excel.Worksheet)workBook.Sheets[2];
            xlRange2 = dataSheet.UsedRange;
            Thread.Sleep(1000);
        }
        [Test]
        public void Open()
        {
            Console.WriteLine(xlRange.Cells[19, 2].value.ToString());
        }
        [Test]
        public void TestLogin()
        {
            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='username']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='username']"));
            Email.SendKeys(xlRange.Cells[19, 2].value.ToString());

            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(xlRange.Cells[19, 3].value.ToString());

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            var expected = xlRange.Cells[19, 4].value.ToString();
            IWebElement actual = driver.FindElement(By.XPath("//h1[contains(text(),'Chào mừng đến với trang quản trị')]"));
            Assert.That(expected, Is.EqualTo(actual.Text));
        }

        [Test]
        public void TestXemNguoiDung()
        {
            string email = xlRange.Cells[18, 2].value.ToString();
            string password = xlRange.Cells[18, 3].value.ToString();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='username']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='username']"));
            Email.SendKeys(email);

            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(password);

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            IWebElement DanhSach = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a"));
            DanhSach.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[1]/a")).Displayed);

            IWebElement DSKhachHang = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[1]/a"));
            DSKhachHang.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            Thread.Sleep(2000);

            IWebElement DanhSach1 = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a"));
            DanhSach1.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[2]/a")).Displayed);

            IWebElement DSNhanVien = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[2]/a"));
            DSNhanVien.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);
        }
        [Test]
        public void TestXemSanPham()
        {
            string email = xlRange.Cells[18, 2].value.ToString();
            string password = xlRange.Cells[18, 3].value.ToString();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='username']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='username']"));
            Email.SendKeys(email);

            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(password);

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            IWebElement SanPham = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[3]/a"));
            SanPham.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[3]/ul/li[1]/a")).Displayed);

            IWebElement DSSanPham = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[3]/ul/li[1]/a"));
            DSSanPham.Click();

        }
        [Test]
        public void TestXoaSanPham()
        {
            string email = "admin@gmail.com";
            string password = "1";

            IWebElement SignIn = driver.FindElement(By.XPath("//a[normalize-space()='Sign in']"));
            SignIn.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='email']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='email']"));
            Email.SendKeys(email);

            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(password);

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            IWebElement SanPham = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[3]/a"));
            SanPham.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[3]/ul/li[1]/a")).Displayed);

            IWebElement DSSanPham = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[3]/ul/li[1]/a"));
            DSSanPham.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[3]/header/h1")).Displayed);

            IWebElement XoaSP = driver.FindElement(By.XPath("//*[@id=\"productTable\"]/tbody/tr[1]/td[6]/a[2]"));
            XoaSP.Click();
        }
        [Test]
        public void TestXoaNhanVien()
        {
            string email = xlRange.Cells[18, 2].value.ToString();
            string password = xlRange.Cells[18, 3].value.ToString();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='username']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='username']"));
            Email.SendKeys(email);

            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(password);

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            IWebElement DanhSach1 = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a"));
            DanhSach1.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[2]/a")).Displayed);

            IWebElement DSNhanVien = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[2]/a"));
            DSNhanVien.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            IWebElement Xoa = driver.FindElement(By.XPath("/html/body/div[3]/section/table/tbody/tr[2]/td[5]/a[2]"));
            Xoa.Click();

            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
        }



        [Test]
        public void TestThemNhanVien()
        {
            int a = 23;
            string email = xlRange.Cells[a, 2].value.ToString();
            string password = xlRange.Cells[a, 3].value.ToString();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='username']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='username']"));
            Email.SendKeys(email);

            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(password);

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            IWebElement DanhSach1 = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a"));
            DanhSach1.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[2]/a")).Displayed);

            IWebElement ThemNV = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[3]/a"));
            ThemNV.Click();

            IWebElement TenNV = driver.FindElement(By.XPath("//*[@id=\"username\"]"));
            TenNV.SendKeys(xlRange.Cells[a,4].value.ToString());

            IWebElement EmailNV = driver.FindElement(By.XPath("//*[@id=\"email\"]"));
            EmailNV.SendKeys(xlRange.Cells[a, 5].value.ToString());

            IWebElement PhoneNV = driver.FindElement(By.XPath("//*[@id=\"phone\"]"));
            PhoneNV.SendKeys(xlRange.Cells[a, 6].value.ToString());

            IWebElement AddressNV = driver.FindElement(By.XPath("//*[@id=\"address\"]"));
            AddressNV.SendKeys(xlRange.Cells[a, 7].value.ToString());

            IWebElement PasswordNV = driver.FindElement(By.XPath("//*[@id=\"password\"]"));
            PasswordNV.SendKeys(xlRange.Cells[a, 8].value.ToString());

            IWebElement RoleNV = driver.FindElement(By.XPath("//*[@id=\"role\"]"));
            SelectElement categorySelect = new SelectElement(RoleNV);
            categorySelect.SelectByValue("Quản Lý");

            IWebElement Submit = driver.FindElement(By.XPath("/html/body/div[3]/section/form/input[6]"));
            Submit.Click();
        }
        [Test]
        public void TestDashboard()    //Test sự thay đổi số lượng nhân viên
        {
            string email = "admin@gmail.com";
            string password = "1";

            IWebElement SignIn = driver.FindElement(By.XPath("//a[normalize-space()='Sign in']"));
            SignIn.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='email']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='email']"));
            Email.SendKeys(email);

            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(password);

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            Thread.Sleep(2000);

            IWebElement initialUserCountElement = driver.FindElement(By.XPath("/html/body/div[3]/div/section/div[2]/p"));
            string initialUserCountText = initialUserCountElement.Text;
            int initialUserCount = int.Parse(initialUserCountText);



            IWebElement DanhSach1 = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a"));
            DanhSach1.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[2]/a")).Displayed);

            IWebElement ThemNV = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/ul/li[3]/a"));
            ThemNV.Click();

            IWebElement TenNV = driver.FindElement(By.XPath("//*[@id=\"username\"]"));
            TenNV.SendKeys("Nguyễn Gian Lậu");

            IWebElement EmailNV = driver.FindElement(By.XPath("//*[@id=\"email\"]"));
            EmailNV.SendKeys("gianquadi@gmail.com");

            IWebElement PhoneNV = driver.FindElement(By.XPath("//*[@id=\"phone\"]"));
            PhoneNV.SendKeys("099992221");

            IWebElement AddressNV = driver.FindElement(By.XPath("//*[@id=\"address\"]"));
            AddressNV.SendKeys("13A/1 Duong1, Linh Xuan, Thu Duc, TP.HCM");

            IWebElement PasswordNV = driver.FindElement(By.XPath("//*[@id=\"password\"]"));
            PasswordNV.SendKeys("123456");

            IWebElement RoleNV = driver.FindElement(By.XPath("//*[@id=\"role\"]"));
            SelectElement categorySelect = new SelectElement(RoleNV);
            categorySelect.SelectByValue("Nhân Viên");

            IWebElement Submit = driver.FindElement(By.XPath("/html/body/div[3]/section/form/input[6]"));
            Submit.Click();

            Thread.Sleep(2000);

            driver.Navigate().GoToUrl("http://localhost:44369/Admin/Dashboard");

            //IWebElement updatedUserCountElement = driver.FindElement(By.XPath("/html/body/div[3]/div/section/div[2]/p"));
            //string updatedUserCountText = updatedUserCountElement.Text;
            //int updatedUserCount = int.Parse(updatedUserCountText);

            //Assert.That(initialUserCount + 1, updatedUserCount.ToString(), "Số lượng người dùng không tăng sau khi thêm người dùng mới.");
        }

        [Test]
        public void TestThaoTacDonHang()
        {
            string email = "admin@gmail.com";
            string password = "1";

            IWebElement SignIn = driver.FindElement(By.XPath("//a[normalize-space()='Sign in']"));
            SignIn.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='email']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='email']"));
            Email.SendKeys(email);

            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(password);

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[2]/ul/li[2]/a")).Displayed);

            Thread.Sleep(2000);


            IWebElement DonHang = driver.FindElement(By.XPath("/html/body/div[2]/ul/li[4]/a"));
            DonHang.Click();

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[3]/div[2]/section/div/table/tbody/tr[4]/td[6]/button")).Displayed);


            IWebElement TimDonHang = driver.FindElement(By.XPath("//*[@id=\"searchOrder\"]"));
            TimDonHang.SendKeys("14");

            wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[3]/div[2]/section")).Displayed);

            IWebElement DoiTrangThai = driver.FindElement(By.XPath("/html/body/div[3]/div[2]/section/div/table/tbody/tr[8]/td[5]/select"));
            SelectElement categorySelect = new SelectElement(DoiTrangThai);
            categorySelect.SelectByValue("Đang xử lý");

            Thread.Sleep(2000);

            IWebElement XoaDonHang = driver.FindElement(By.XPath("/html/body/div[3]/div[2]/section/div/table/tbody/tr[8]/td[6]/button"));
            XoaDonHang.Click();

            IAlert alert1 = driver.SwitchTo().Alert();
            alert1.Accept();

            Thread.Sleep(2000);

        }
        [TearDown]
        public void CloseWeb()
        {
            Thread.Sleep(2000);
            driver.Close();
            workBook.Save();
            workBook.Close();
        }
    }
}
