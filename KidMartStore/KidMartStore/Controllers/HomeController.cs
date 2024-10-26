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

        public ActionResult Index(Khách_Hàng KhachHang)
        {
            return View();
        }
    }
}