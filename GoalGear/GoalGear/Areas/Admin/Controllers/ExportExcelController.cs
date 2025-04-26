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

