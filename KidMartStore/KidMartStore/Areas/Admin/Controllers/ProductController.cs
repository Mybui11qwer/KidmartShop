using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidMartStore.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public KidMartStoreEntities database = new KidMartStoreEntities();

        //Thêm
        public ActionResult AddNewProduct()
        {
            List<Category> categories = database.Categories.ToList();
            return View(categories);
        }
        [HttpPost]
        public ActionResult AddNewProduct(Product NewProduct)
        {
            try
            {
                if (NewProduct.UploadImage != null && NewProduct.UploadImage.ContentLength > 0)
                {
                    try
                    {
                        string newFileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(NewProduct.UploadImage.FileName);
                        string NewName = newFileName + extension;
                        string FileName = Path.GetFileName(NewName);
                        string path = Path.Combine(Server.MapPath("~/Image/Product/"), FileName);

                        string externalFolder = @"D:\ShopBanQuanAo\KidMartStore\KidMartStore\Image\Product\";
                        string externalPath = Path.Combine(externalFolder, FileName);
                        NewProduct.Image = FileName;
                        NewProduct.UploadImage.SaveAs(path);
                        NewProduct.UploadImage.SaveAs(externalPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi lưu tệp: " + ex.Message);
                    }
                }

                database.Products.Add(NewProduct);
                database.SaveChanges();
                return RedirectToAction("ManagerProduct", "Admin", new { area = "Admin" });
            }
            catch
            {
                return View("AddNewProduct");
            }
        }
        //Sửa 
        public ActionResult UpdateProduct(int id)
        {
            var product = database.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpPost]
        public ActionResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = database.Products.Find(product.ID_Product);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Quantity = product.Quantity;

                    database.SaveChanges();
                }
            }
            return RedirectToAction("ManagerProduct", "Home", new { area = "Admin" });
        }

        //Xóa
        public ActionResult DeleteAccount(int id)
        {
            var product = database.Products.Find(id);
            if (product != null)
            {
                database.Products.Remove(product);
                database.SaveChanges();
            }
            return RedirectToAction("ManagerProduct", "Home", new { area = "Admin" });
        }
    }
}