using KidMartStore.Models;
using KidMartStore.Patterns.Command;
using KidMartStore.Patterns.Singleton;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidMartStore.Controllers
{
    public class FunctionUserController : Controller
    {
        private readonly KidMartStoreEntities database = DatabaseContextSingleton.Instance;

        //Hủy đơn hàng.
        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            //Sử dụng mẫu command
            var command = new CancelOrderCommand(database, id);
            if (command.Execute())
            {
                TempData["Message"] = "Đơn hàng đã được hủy thành công!";
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn hàng này!";
            }
            return RedirectToAction("ShowOrder", "Home");
        }
    }
}