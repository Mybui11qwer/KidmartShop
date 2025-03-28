using KidMartStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Patterns.Command
{
    public class CancelOrderCommand
    {
        private readonly KidMartStoreEntities _db;
        private readonly int _orderId;

        public CancelOrderCommand(KidMartStoreEntities db, int orderId)
        {
            _db = db;
            _orderId = orderId;
        }

        public bool Execute()
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var order = _db.Orders.FirstOrDefault(o => o.ID_Order == _orderId);
                    if (order == null || order.Status == "Đã giao")
                        return false;

                    var orderDetails = _db.Detail_Order.Where(od => od.ID_Order == _orderId).ToList();
                    foreach (var item in orderDetails)
                    {
                        var product = _db.Products.FirstOrDefault(p => p.ID_Product == item.ID_Product);
                        if (product != null)
                        {
                            product.Quantity += item.Quantity;
                        }
                    }
                    order.Status = "Đã hủy";
                    _db.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}