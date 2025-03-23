using Microsoft.Office.Interop.Excel;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace TestProject1
{
    public class Tests
    {
        private Application? dataApp;
        private Workbook? workBook;
        private Worksheet? dataSheet;

        [SetUp]
        public void Setup()
        {
            try
            {
                // Khởi tạo ứng dụng Excel
                dataApp = new Application();

                string filePath = @"D:\Nhom_3.xlsx";
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File không tồn tại!");
                    return;
                }

                workBook = dataApp.Workbooks.Open(filePath);

                // Kiểm tra xem file có ít nhất 3 sheet không
                if (workBook.Sheets.Count < 3)
                {
                    Console.WriteLine("File Excel không có đủ sheet!");
                    return;
                }

                dataSheet = (Worksheet)workBook.Sheets[3];

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi mở file Excel: {ex.Message}");
            }
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [TearDown]
        public void Cleanup()
        {
            // Giải phóng tài nguyên
            try
            {
                workBook?.Close(false);
                dataApp?.Quit();

                // Giải phóng bộ nhớ (quan trọng)
                if (workBook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
                if (dataApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(dataApp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đóng Excel: {ex.Message}");
            }
        }
    }
}
