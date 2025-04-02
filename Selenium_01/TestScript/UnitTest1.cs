using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestScript
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_CalculateTotalMoney()
        {
            // Arrange: Tạo giỏ hàng thông qua controller (mô phỏng như trong ứng dụng thực tế)
            var controller = new FunctionCartController();

            // Giả lập session
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new HttpContextWrapper(new HttpContext(new HttpRequest("", "http://localhost", ""), new HttpResponse(null)))
            };

            // Giả lập người dùng đã đăng nhập
            controller.Session["ID_Customer"] = 1; // ID giả lập của khách hàng

            // Thêm sản phẩm vào giỏ hàng
            controller.AddToCart(1, 2); // Thêm 2 sản phẩm có ID = 1 vào giỏ hàng
            controller.AddToCart(2, 1); // Thêm 1 sản phẩm có ID = 2 vào giỏ hàng

            // Lấy giỏ hàng từ session sau khi thêm sản phẩm
            var cart = controller.GetCart();

            // Act: Gọi phương thức tính tổng tiền
            decimal total = cart.TotalMoney();

            // Assert: Kiểm tra tổng tiền có đúng không
            Assert.AreEqual(200000m, total, "Tổng tiền không đúng.");
        }

    }
}
