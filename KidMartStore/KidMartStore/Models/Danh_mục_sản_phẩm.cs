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
    
    public partial class Danh_mục_sản_phẩm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Danh_mục_sản_phẩm()
        {
            this.Sản_phẩm = new HashSet<Sản_phẩm>();
        }
    
        public int Mã_danh_mục { get; set; }
        public string Tên_danh_mục { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sản_phẩm> Sản_phẩm { get; set; }
    }
}
