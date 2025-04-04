//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KidMartStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Detail_Order = new HashSet<Detail_Order>();
            this.Detail_Order1 = new HashSet<Detail_Order>();
        }
    
        public int ID_Order { get; set; }
        public int ID_Customer { get; set; }
        public System.DateTime Order_Date { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public Nullable<int> ID_Sale { get; set; }
        public string PaymentMethod { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Customer Customer1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Order> Detail_Order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detail_Order> Detail_Order1 { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
