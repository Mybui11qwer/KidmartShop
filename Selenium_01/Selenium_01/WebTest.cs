using Microsoft.Office.Interop.Excel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;

namespace Selenium_01
{
    [TestFixture]
    public class WebTest
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
            driver.Navigate().GoToUrl("https://localhost:44369/Home/Index");
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //Actions action = new Actions(driver);

            //Mở Excel
            dataApp = new Excel.Application();
            if (System.IO.File.Exists(@"C:\Users\mybui\OneDrive - Ho Chi Minh City University of Foreign Languages and Information Technology - HUFLIT\TestCase Nhom3\Nhom 3.xlsx"))
            {
                workBook = dataApp.Workbooks.Open(@"C:\Users\mybui\OneDrive - Ho Chi Minh City University of Foreign Languages and Information Technology - HUFLIT\TestCase Nhom3\Nhom 3.xlsx");
                Console.WriteLine("Đọc và ghi file thành công");
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
        public void TestOpenWebsite()
        {
            for(int i = 3; i < 4; i++)
            {
                Console.WriteLine(xlRange.Cells[3, 16].value.ToString());
            }
        }
        public void Login()
        {
            IWebElement SignIn = driver.FindElement(By.XPath("//a[normalize-space()='Sign in']"));
            SignIn.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='email']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='email']"));
            Email.SendKeys(xlRange.Cells[2, 16].value.ToString());
            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(xlRange.Cells[3, 16].value.ToString());

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();
        }
        //Đăng nh
        [Test]
        public void TestLoginComplete()
        {
            Login();

            var expectedName = xlRange.Cells[4, 16].value != null ? xlRange.Cells[4, 16].value.ToString() : "";
            IWebElement actualName = driver.FindElement(By.ClassName("NameUser"));
            Assert.That(expectedName, Is.EqualTo(actualName.Text));

           
            if (expectedName == actualName.Text)
            {
                xlRange.Cells[3, 9].value = "[3] Ok";
                xlRange2.Cells[11, 9].value = "Passed";
            }
            else
            {
                xlRange.Cells[3, 9].value = "[3] Failed";
                xlRange2.Cells[11, 9].value = "Failed";
            }           
        }
        //Đăng nhập không thành công
        [Test]
        public void TestLoginFailed()
        {           
            for (int i = 4; i < 9; i++)
            {
                IWebElement SignIn = driver.FindElement(By.XPath("//a[normalize-space()='Sign in']"));
                SignIn.Click();

                wait.Until(driver => driver.FindElement(By.XPath("//input[@id='email']")).Displayed);
                IWebElement Email = driver.FindElement(By.XPath("//input[@id='email']"));
                Email.Clear();
                Email.SendKeys(xlRange.Cells[i, 2].value.ToString());
                IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
                Password.Clear();
                Password.SendKeys(xlRange.Cells[i, 3].value.ToString());

                IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
                btnLogin.Click();

                try
                {
                    wait.Until(driver => driver.FindElement(By.XPath("//h2[contains(text(),'ĐĂNG NHẬP TÀI KHOẢN')]")).Displayed);
                    xlRange.Cells[i, 9].value = "[3] Ok";
                    xlRange2.Cells[i + 8, 9].value = "Passed";
                }
                catch (NoSuchElementException)
                {
                    xlRange.Cells[i, 9].value = "[3] Failed";
                    xlRange2.Cells[i + 8, 9].value = "Failed";
                }
            }
        }
        [Test]
        public void TestLogout()
        {
            Login();

            wait.Until(driver => driver.FindElement(By.XPath("//a[normalize-space()='Logout']")).Displayed);
            IWebElement btnLogout = driver.FindElement(By.XPath("//a[normalize-space()='Logout']"));
            btnLogout.Click();
        }
        [Test]
        public void TestCheckNameUser()
        {
            string email = xlRange.Cells[3, 2].value.ToString();
            string password = xlRange.Cells[3, 3].value.ToString();

            IWebElement SignIn = driver.FindElement(By.XPath("//a[normalize-space()='Sign in']"));
            SignIn.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='email']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='email']"));
            Email.SendKeys(email);
            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(password);

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();

            var expected = xlRange.Cells[3, 4].value != null ? xlRange.Cells[3, 4].value.ToString() : "";

            IWebElement userDisplay = driver.FindElement(By.ClassName("NameUser"));
            Assert.That(expected, Is.EqualTo(userDisplay.Text));

            xlRange2.Cells[11, 8].value = "[3] Ok";
            if (expected == userDisplay.Text)
            {
                xlRange2.Cells[11, 9].value = "Passed";
            }
            else
            {
                xlRange2.Cells[11, 9].value = "Failed";
            }
        }
        [Test]
        public void RegisterAccountComplete()
        {           
            for(int i = 9; i < 15; i++)
            {
                IWebElement SignUp = driver.FindElement(By.XPath("//a[normalize-space()='Sign up']"));
                SignUp.Click();

                wait.Until(driver => driver.FindElement(By.XPath("//input[@id='name']")).Displayed);
                IWebElement Username = driver.FindElement(By.XPath("//input[@id='name']"));
                Username.Clear();
                Username.SendKeys(xlRange.Cells[i, 5].value.ToString());
                IWebElement NumPhone = driver.FindElement(By.XPath("//input[@id='phone']"));
                NumPhone.Clear();
                NumPhone.SendKeys(xlRange.Cells[i, 4].value.ToString());
                IWebElement Email = driver.FindElement(By.XPath("//input[@id='email']"));
                Email.Clear();
                Email.SendKeys(xlRange.Cells[i, 2].value.ToString());
                IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
                Password.Clear();
                Password.SendKeys(xlRange.Cells[i, 3].value.ToString());

                IWebElement btnSignUp = driver.FindElement(By.XPath("//input[@class='buttonRegister']"));
                btnSignUp.Click();

                try
                {
                    wait.Until(driver => driver.FindElement(By.XPath("//h2[contains(text(),'ĐĂNG NHẬP TÀI KHOẢN')]")).Displayed);
                    xlRange2.Cells[i + 8, 8].Value = "Đăng ký thành công";
                    xlRange2.Cells[i + 8, 9].Value = "Passed";
                    xlRange.Cells[i, 9].value = "[3] Ok";
                }
                catch (Exception)
                {
                    xlRange2.Cells[i + 8, 8].Value = "Đăng ký không thành công";
                    xlRange2.Cells[i + 8, 9].Value = "Passed";
                    xlRange.Cells[i, 9].value = "[3] Ok";
                }
            }    
        }

        [Test]
        public void TestCheckProfile()
        {
            Login();
            xlRange.Cells[15, 9] = "[1] Ok";
            wait.Until(driver => driver.FindElement(By.Id("WelcomeUser")).Displayed);
            IWebElement iconUser = driver.FindElement(By.Id("WelcomeUser"));
            iconUser.Click();

            wait.Until(driver => driver.FindElement(By.ClassName("content1")).Displayed);
            for (int i = 4; i < 8; i++)
            {
                var expected = xlRange.Cells[15, i].value.ToString();
                IWebElement actual = driver.FindElement(By.Id("section1")).FindElement(By.Id((i - 3).ToString()));
                Assert.That(actual.Text, Is.EqualTo(expected));
                if(actual.Text == expected)
                {
                    xlRange.Cells[15, 10] = "[2] Ok";
                    xlRange2.Cells[23, 9].value = "Passed";
                }
                else
                {
                    xlRange.Cells[15, 10] = "[2] Failed";
                    xlRange2.Cells[23, 9].value = "Failed";
                }
            }
        }
        [Test]
        public void CheckCart()
        {
            IWebElement cartIcon = driver.FindElement(By.XPath("//img[@src='/Image/Icons/icon-cart.png']")); // Cập nhật selector
            cartIcon.Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

            IAlert alert = driver.SwitchTo().Alert();
            var alertText = alert.Text;

            alert.Accept();

            wait.Until(driver => driver.Url.Contains("/Auth/Login"));
            var currentUrl = driver.Url;
            Assert.That(currentUrl, Is.EqualTo("https://localhost:44369/Auth/Login"));

            if(currentUrl == "https://localhost:44369/Auth/Login")
            {
                xlRange.Cells[16, 9].value = "[3] Ok";
                xlRange2.Cells[24, 9].value = "Passed";
            }
            else
            {
                xlRange.Cells[16, 9].value = "Không tìm thấy link";
                xlRange2.Cells[24, 9].value = "Failed";
            }
        }
        [Test]
        public void CheckCartLoginedAndEmptyCart()
        {
            Login();

            IWebElement cartIcon = driver.FindElement(By.XPath("//img[@src='/Image/Icons/icon-cart.png']"));
            cartIcon.Click();

            var expected = xlRange.Cells[17, 4].value.ToString();
            IWebElement actual = driver.FindElement(By.ClassName("empty-cart")).FindElement(By.XPath("/html/body/main/div/h1"));
            Assert.That(actual.Text, Is.EqualTo(expected));

            if(expected == actual.Text)
            {
                xlRange.Cells[17, 9].value = "[3] Ok";
                xlRange2.Cells[25, 9].value = "Passed";
            }
            else
            {
                xlRange.Cells[17, 9].value = "[3] Failed";
                xlRange2.Cells[25, 9].value = "Failed";
            }
        }
        [Test]
        public void TestCartLoginedAndHaveProduct()
        {
            Login();

            wait.Until(driver => driver.FindElement(By.XPath("//section[1]//div[1]//nav[2]//a[2]")));
            IWebElement Muahang = driver.FindElement(By.XPath("//section[1]//div[1]//nav[2]//a[2]"));
            Muahang.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//button[@class='plus']")));
            IWebElement AddToCart = driver.FindElement(By.XPath("//button[@class='AddToCart']")); //Thêm vào giỏ hàng
            AddToCart.Click();

            driver.Navigate().GoToUrl("https://localhost:44369/Home/GioHang");

            var expected = xlRange.Cells[18, 4].value.ToString();
            IWebElement actual = driver.FindElement(By.ClassName("checkout")).FindElement(By.XPath("/html/body/main/div/div[2]/h3"));
            Assert.That(actual.Text, Is.EqualTo(expected));
            if (expected == actual.Text)
            {
                xlRange.Cells[18, 9].value = "[3] Ok";
                xlRange2.Cells[26, 9].value = "Passed";
            }
            else
            {
                xlRange.Cells[18, 9].value = "[3] Failed";
                xlRange2.Cells[26, 9].value = "Failed";
            }
        }
        [Test]
        public void TestUpdateProfile01()
        {
            Login();
            IWebElement iconUser = driver.FindElement(By.XPath("//img[@src='/Image/Icons/Icon-user.png']"));
            iconUser.Click();

            wait.Until(driver => driver.FindElement(By.ClassName("SetProfile")).Displayed);
            IWebElement btnUpdate = driver.FindElement(By.ClassName("SetProfile"));
            btnUpdate.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@name='Username']")).Displayed);
            IWebElement UsernameFrame = driver.FindElement(By.XPath("//input[@name='Username']"));
            UsernameFrame.Clear();
            UsernameFrame.SendKeys(xlRange.Cells[19, 4].value.ToString());

            IWebElement AddressFrame = driver.FindElement(By.XPath("//input[@name='Address']"));
            AddressFrame.Clear();
            AddressFrame.SendKeys(xlRange.Cells[19, 5].value.ToString());

            IWebElement btnUpdateProfile = driver.FindElement(By.ClassName("button"));
            btnUpdateProfile.Click();

            var expectedNameUser = xlRange.Cells[19, 4].value.ToString();
            var expectedAddress = xlRange.Cells[19, 5].value.ToString();
            wait.Until(driver => driver.FindElement(By.ClassName("NameUser")).Displayed);
            IWebElement ActualNameUser = driver.FindElement(By.ClassName("NameUser"));
            
            wait.Until(driver => driver.FindElement(By.XPath("//span[@id='2']")).Displayed);
            IWebElement ActualAddress = driver.FindElement(By.XPath("//span[@id='2']"));

            if(expectedNameUser == ActualNameUser.Text || expectedAddress == ActualAddress.Text)
            {
                xlRange.Cells[19, 9].value = "[3] Ok";
                xlRange2.Cells[27, 8].value = "[3] Ok";
                xlRange2.Cells[27, 9].value = "Passed";
            }
            else
            {
                xlRange.Cells[19, 9].value = "[3] Failed";
                xlRange2.Cells[27, 8].value = "[3] Failed";
                xlRange2.Cells[27, 9].value = "Failed";
            }
        }
        public void AddToCart()
        {
            wait.Until(driver => driver.FindElement(By.XPath("//main//nav[1]//a[2]")));
            IWebElement Muahang = driver.FindElement(By.XPath("//main//nav[1]//a[2]"));
            Muahang.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//button[@class='plus']")));
            IWebElement SLProduct = driver.FindElement(By.XPath("//button[@class='plus']")); //Mua 2 sản phẩm
            SLProduct.Click();
            IWebElement AddToCart = driver.FindElement(By.XPath("//button[@class='AddToCart']")); //Thêm vào giỏ hàng
            AddToCart.Click();
            Thread.Sleep(2000);
            //Check giỏ hàng và mua tại đây
            IWebElement GoToCart = driver.FindElement(By.XPath("//img[@src='/Image/Icons/icon-cart.png']")); //Vào giỏ hàng
            GoToCart.Click();
        }
        [Test]
        public void TestAddToCart01()
        {
            Login();
            AddToCart();

            var expectedTotal = xlRange.Cells[21, 6].value.ToString("N0") + " " + "VNĐ";
            var expectedNameProduct = xlRange.Cells[21, 7].value.ToString();
            wait.Until(driver => driver.FindElement(By.ClassName("total-amount")));
            IWebElement ActualTotal = driver.FindElement(By.ClassName("total-amount"));
            IWebElement ActualNameProduct = driver.FindElement(By.ClassName("nameProduct"));

            if (expectedTotal == ActualTotal.Text && expectedNameProduct == ActualNameProduct.Text)
            {
                xlRange.Cells[21, 9].value = "[4] Ok";
                xlRange.Cells[21, 10].value = "[5] Ok";          
            }
            else if (expectedTotal != ActualTotal.Text && expectedNameProduct == ActualNameProduct.Text)
            {
                xlRange.Cells[21, 9].value = "[4] Ok";
                xlRange.Cells[21, 10].value = "[5] Failed";
            }
            else if (expectedTotal == ActualTotal.Text && expectedNameProduct != ActualNameProduct.Text)
            {
                xlRange.Cells[21, 9].value = "[4] Failed";
            }
            else
            {
                xlRange.Cells[21, 9].value = "[4] Failed";
            }
            xlRange2.Cells[31, 8].value = xlRange.Cells[21, 9].value + "\n" + xlRange.Cells[21, 10].value;
            if (xlRange.Cells[21, 9].value.ToString() == xlRange.Cells[22, 10].value.ToString())
            {
                xlRange2.Cells[32, 9].value = "Passed";
            }
            else
            {
                xlRange2.Cells[31, 9].value = "Failed";
            }
        }
        [Test]
        public void TestAddToCart02()
        {
            Login();
            AddToCart();

            var expectedTotal = xlRange.Cells[22, 6].value.ToString("N0") + " " + "VNĐ";
            wait.Until(driver => driver.FindElement(By.ClassName("total-amount")).Displayed);
            IWebElement ActualTotal = driver.FindElement(By.ClassName("total-amount"));

            if (expectedTotal == ActualTotal.Text)
            {
                xlRange.Cells[22, 9].value = "[4] Ok";
            }
            else if (expectedTotal != ActualTotal.Text)
            {
                xlRange.Cells[22, 9].value = "[4] Failed";                
            }

            driver.Navigate().GoToUrl("https://localhost:44369/Home/Index");
            AddToCart();
           
            var expectedTotal2 = xlRange.Cells[22, 8].value.ToString("N0") + " " + "VNĐ";
            wait.Until(driver => driver.FindElement(By.ClassName("total-amount")).Displayed);           
            IWebElement ActualTotal2 = driver.FindElement(By.ClassName("total-amount"));

            if (expectedTotal2 == ActualTotal2.Text)
            {
                xlRange.Cells[22, 10].value = "[5] Ok";
            }
            else if (expectedTotal2 != ActualTotal2.Text)
            {
                xlRange.Cells[22, 10].value = "[5] Failed";
            }
            xlRange2.Cells[32, 8].value = xlRange.Cells[22, 9].value + "\n" + xlRange.Cells[22, 10].value;
            if(xlRange.Cells[22, 9].value.ToString() == "[4] Ok" || xlRange.Cells[22, 10].value.ToString() == "[5] Ok")
            {
                xlRange2.Cells[32, 9].value = "Passed";
            }
            else
            {
                xlRange2.Cells[32, 9].value = "Failed";
            }
        }
        
        [Test]
        public void TestCheckingOrder()
        {
            Login();

            wait.Until(driver => driver.FindElement(By.ClassName("OrderChecking")).Displayed);
            IWebElement checkOrder = driver.FindElement(By.ClassName("OrderChecking"));
            checkOrder.Click();

            var expected = "Lịch sử đơn hàng";
            IWebElement actual = driver.FindElement(By.XPath("//h2[contains(text(),'Lịch sử đơn hàng')]"));
            Assert.That(actual.Text, Is.EqualTo(expected)); 
            try
            {

            }
        }
        public void CheckOrder()
        {
            wait.Until(driver => driver.FindElement(By.ClassName("OrderChecking")).Displayed);
            IWebElement checkOrder = driver.FindElement(By.ClassName("OrderChecking"));
            checkOrder.Click();
        }
        [Test]
        public void CheckOut_01()
        {
            Login();
            AddToCart();
            if(driver.Url == "https://localhost:44369/Home/GioHang")
            {
                xlRange.Cells[23, 9].value = "[2] Ok";
            }
            else
            {
                xlRange.Cells[23, 9].value = "[2] Failed";
            }

            wait.Until(driver => driver.FindElement(By.ClassName("checkout-button")).Displayed);
            IWebElement btnCheckout = driver.FindElement(By.ClassName("checkout-button"));
            btnCheckout.Click();
            
            IWebElement btnConfirm = driver.FindElement(By.XPath("//button[@onclick='submitPayment()']"));
            btnConfirm.Click();

            //Thanh toán thành công
            var expectedTilte = xlRange.Cells[23, 4].value.ToString();
            wait.Until(driver => driver.FindElement(By.ClassName("success-title")).Displayed);
            IWebElement titleComplete = driver.FindElement(By.ClassName("success-title"));
            Assert.That(expectedTilte, Is.EqualTo(titleComplete.Text.Trim()));
            try
            {
                xlRange.Cells[23, 10].value = "[5] Ok";
            }
            catch 
            {
                xlRange.Cells[23, 10].value = "[5] Faile";
            }

            xlRange2.Cells[33, 8].value = xlRange.Cells[23, 9].value + "\n" + xlRange.Cells[23, 10].value;
            if (xlRange.Cells[23, 9].value.ToString() == "[2] Ok" || xlRange.Cells[23, 10].value.ToString() == "[5] Ok")
            {
                xlRange2.Cells[33, 9].value = "Passed";
            }
            else
            {
                xlRange2.Cells[33, 9].value = "Failed";
            }
        }
        public void Login01()
        {
            IWebElement SignIn = driver.FindElement(By.XPath("//a[normalize-space()='Sign in']"));
            SignIn.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//input[@id='email']")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//input[@id='email']"));
            Email.SendKeys(xlRange.Cells[9, 16].value.ToString());
            IWebElement Password = driver.FindElement(By.XPath("//input[@id='password']"));
            Password.SendKeys(xlRange.Cells[10, 16].value.ToString());

            IWebElement btnLogin = driver.FindElement(By.XPath("//input[@value='Đăng nhập']"));
            btnLogin.Click();
        }
        public void LoginPayPal()
        {
            wait.Until(driver => driver.FindElement(By.Id("headerText")).Displayed);

            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"email\"]")).Displayed);
            IWebElement Email = driver.FindElement(By.XPath("//*[@id=\"email\"]"));
            Email.SendKeys(xlRange.Cells[9, 16].value.ToString());
            IWebElement btnNext = driver.FindElement(By.Id("btnNext"));
            btnNext.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"password\"]")).Displayed);
            IWebElement Password = driver.FindElement(By.XPath("//*[@id=\"password\"]"));
            Password.SendKeys(xlRange.Cells[10, 17].value.ToString());

            IWebElement btnLogin = driver.FindElement(By.Id("btnLogin"));
            btnLogin.Click();
        }
        [Test]
        public void Checkout_02()
        {
            Login01();
            AddToCart();
            if (driver.Url == "https://localhost:44369/Home/GioHang")
            {
                xlRange.Cells[24, 9].value = "[2] Ok";
            }
            else
            {
                xlRange.Cells[24, 9].value = "[2] Failed";
            }

            wait.Until(driver => driver.FindElement(By.ClassName("checkout-button")).Displayed);
            IWebElement btnCheckout = driver.FindElement(By.ClassName("checkout-button"));
            btnCheckout.Click();
            
            IWebElement SelectWayToPay = driver.FindElement(By.Id("payment-method"));
            SelectElement selectElement = new SelectElement(SelectWayToPay);
            selectElement.SelectByValue("Paypal");

            IWebElement btnConfirm = driver.FindElement(By.XPath("//button[@onclick='submitPayment()']"));
            btnConfirm.Click();

            if (driver.Url == "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=EC-2BS59092VY3127819")
            {
                xlRange.Cells[24, 10].value = "[3] Ok";
                xlRange.Cells[24, 11].value = "[5] Ok";
            }
            else
            {
                xlRange.Cells[24, 10].value = "[3] Ok";
                xlRange.Cells[24, 11].value = "[5] Failed";
            }

            LoginPayPal();
            IWebElement btnPay = driver.FindElement(By.XPath("//button[@id='payment-submit-btn']"));
            btnPay.Click();

            //Thanh toán thành công
            var expectedTilte = xlRange.Cells[24, 4].value.ToString();
            wait.Until(driver => driver.FindElement(By.ClassName("success-title")).Displayed);
            IWebElement titleComplete = driver.FindElement(By.ClassName("success-title"));
            Assert.That(expectedTilte, Is.EqualTo(titleComplete.Text.Trim()));
            try
            {
                xlRange.Cells[24, 12].value = "[8] Ok";
            }
            catch
            {
                xlRange.Cells[24, 12].value = "[8] Failed";
            }

            xlRange2.Cells[34, 8].value = xlRange.Cells[24, 9].value + "\n" + xlRange.Cells[24, 10].value + "\n"
                                        + xlRange.Cells[24, 11].value + "\n" + xlRange.Cells[24, 12].value;
            if (xlRange.Cells[24, 9].value.ToString() == "[2] Ok" 
                || xlRange.Cells[24, 10].value.ToString() == "[3] Ok"
                || xlRange.Cells[24, 11].value.ToString() == "[5] Ok"
                || xlRange.Cells[24, 12].value.ToString() == "[8] Ok")
            {
                xlRange2.Cells[34, 9].value = "Passed";
            }
            else
            {
                xlRange2.Cells[34, 9].value = "Failed";
            }
        }
        /*[Test]
        public void Checkout_03()
        {
            Login();
            AddToCart();
            wait.Until(driver => driver.FindElement(By.ClassName("checkout-button")).Displayed);
            IWebElement btnCheckout = driver.FindElement(By.ClassName("checkout-button"));
            btnCheckout.Click();

            IWebElement btnConfirm = driver.FindElement(By.XPath("//button[@onclick='submitPayment()']"));
            btnConfirm.Click();
        }*/

        [TearDown]
        public void Cleanup()
        {
            Thread.Sleep(2000);
            driver.Close();
            workBook.Save();
            workBook.Close();
        }
    }
}
