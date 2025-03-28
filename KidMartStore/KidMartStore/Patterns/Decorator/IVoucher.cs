using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidMartStore.Patterns.Decorator
{
    public interface IVoucher
    {
        decimal ApplyDiscount(decimal total);
    }

    public class BaseVoucher : IVoucher
    {
        public virtual decimal ApplyDiscount(decimal total)
        {
            return total;
        }
    }

    public class Sale10Voucher : BaseVoucher
    {
        public override decimal ApplyDiscount(decimal total)
        {
            return total * 0.9m; // Giảm 10%
        }
    }

    public class Sale20Voucher : BaseVoucher
    {
        public override decimal ApplyDiscount(decimal total)
        {
            return total * 0.8m; // Giảm 20%
        }
    }
}