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
using System.Globalization;
using System.Data.Entity;

namespace KidMartStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly KidMartStoreEntities database = new KidMartStoreEntities();
        public ActionResult Index()
        {
            List<Product> products = database.Products.ToList();
            return View(products);
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult SanPham()
        {
            List<Product> products = database.Products.ToList();
            return View(products);
        }
    }
}