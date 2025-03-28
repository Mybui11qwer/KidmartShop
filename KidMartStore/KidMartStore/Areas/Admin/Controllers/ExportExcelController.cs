using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using KidMartStore.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace KidMartStore.Areas.Admin.Controllers
{
    // Áp dụng Abstract Factory Pattern cho việc tạo các export khác nhau
    public interface IExportFactory
    {
        byte[] CreateExport();
        string GetContentType();
        string GetFileName();
    }

    // Các factory cụ thể cho từng loại export
    public class OrderExportFactory : IExportFactory
    {
        private readonly KidMartStoreEntities _database;

        public OrderExportFactory(KidMartStoreEntities database)
        {
            _database = database;
        }

        public byte[] CreateExport()
        {
            var orders = _database.Orders.Include(o => o.Customer).Include(o => o.Detail_Order).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Đơn Hàng");

                // Định dạng header
                worksheet.Cells["A1"].Value = "Mã Đơn";
                worksheet.Cells["B1"].Value = "Khách Hàng";
                worksheet.Cells["C1"].Value = "Ngày Đặt";
                worksheet.Cells["D1"].Value = "Tổng Tiền";
                worksheet.Cells["E1"].Value = "Trạng Thái";

                // Style cho header
                using (var range = worksheet.Cells["A1:E1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                int row = 2;
                foreach (var order in orders)
                {
                    worksheet.Cells[row, 1].Value = order.ID_Order;
                    worksheet.Cells[row, 2].Value = order.Customer?.Username;
                    worksheet.Cells[row, 3].Value = order.Order_Date;
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "dd/mm/yyyy";
                    worksheet.Cells[row, 4].Value = order.Total;
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0 đ";
                    worksheet.Cells[row, 5].Value = order.Status;
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        public string GetContentType() => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public string GetFileName() => $"DanhSachDonHang_{DateTime.Now:yyyyMMdd}.xlsx";
    }

    public class ProductExportFactory : IExportFactory
    {
        private readonly KidMartStoreEntities _database;

        public ProductExportFactory(KidMartStoreEntities database)
        {
            _database = database;
        }

        public byte[] CreateExport()
        {
            var products = _database.Products.Include(p => p.Category).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sản Phẩm");

                // Định dạng header
                worksheet.Cells["A1"].Value = "Mã SP";
                worksheet.Cells["B1"].Value = "Tên Sản Phẩm";
                worksheet.Cells["C1"].Value = "Danh Mục";
                worksheet.Cells["D1"].Value = "Giá";
                worksheet.Cells["E1"].Value = "Tồn Kho";

                // Style cho header
                using (var range = worksheet.Cells["A1:E1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkGreen);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                int row = 2;
                foreach (var product in products)
                {
                    worksheet.Cells[row, 1].Value = product.ID_Product;
                    worksheet.Cells[row, 2].Value = product.Name;
                    worksheet.Cells[row, 3].Value = product.Category?.Name_Category;
                    worksheet.Cells[row, 4].Value = product.Price;
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0 đ";
                    worksheet.Cells[row, 5].Value = product.Quantity;
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        public string GetContentType() => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public string GetFileName() => $"DanhSachSanPham_{DateTime.Now:yyyyMMdd}.xlsx";
    }

    public class ExportExcelController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();

        public ActionResult ExportCustomersToExcel()
        {
            var customers = database.Customers
                            .Where(c => c.Role == "Quản Lý" || c.Role == "Nhân Viên")
                            .ToList();
            if (customers.Count == 0)
            {
                return Content("Không có khách hàng nào phù hợp để xuất Excel.");
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Danh sách khách hàng");

                // Thiết lập tiêu đề
                worksheet.Cells["A1:F1"].Merge = true;
                worksheet.Cells["A1"].Value = "DANH SÁCH KHÁCH HÀNG";
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Định dạng cột tiêu đề
                string[] columnHeaders = { "ID", "Tên", "Email", "SĐT", "Địa chỉ", "Quyền" };
                for (int i = 0; i < columnHeaders.Length; i++)
                {
                    worksheet.Cells[2, i + 1].Value = columnHeaders[i];
                    worksheet.Cells[2, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[2, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[2, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Thêm dữ liệu khách hàng vào bảng
                int row = 3;
                foreach (var customer in customers)
                {
                    worksheet.Cells[row, 1].Value = customer.ID_Customer;
                    worksheet.Cells[row, 2].Value = customer.Username;
                    worksheet.Cells[row, 3].Value = customer.Email;
                    worksheet.Cells[row, 4].Value = customer.Phone;
                    worksheet.Cells[row, 5].Value = customer.Address;
                    worksheet.Cells[row, 6].Value = customer.Role;
                    row++;
                }

                // Căn chỉnh cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Xuất file Excel
                byte[] fileContents = excelPackage.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachKhachHang.xlsx");
            }
        }
        public ActionResult ExportOrdersToExcel()
        {
            var orders = database.Orders.ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Danh sách đơn hàng");

                string[] columnHeaders = { "ID", "Khách hàng", "Ngày đặt", "Tổng tiền", "Trạng thái" };
                for (int i = 0; i < columnHeaders.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                int row = 2;
                foreach (var order in orders)
                {
                    worksheet.Cells[row, 1].Value = order.ID_Order;
                    worksheet.Cells[row, 2].Value = order.Customer.Username;
                    worksheet.Cells[row, 3].Value = order.Order_Date.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 4].Value = order.Total;
                    worksheet.Cells[row, 5].Value = order.Status;
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                byte[] fileContents = excelPackage.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachDonHang.xlsx");
            }
        }
    }
}

