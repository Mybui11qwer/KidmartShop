﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KidMartStoreEntities : DbContext
    {
        public KidMartStoreEntities()
            : base("name=KidMartStoreEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Chi_tiết_đơn_hàng> Chi_tiết_đơn_hàng { get; set; }
        public virtual DbSet<Chi_tiết_giỏ_hàng> Chi_tiết_giỏ_hàng { get; set; }
        public virtual DbSet<Danh_mục_sản_phẩm> Danh_mục_sản_phẩm { get; set; }
        public virtual DbSet<Đơn_hàng> Đơn_hàng { get; set; }
        public virtual DbSet<Giảm_giá> Giảm_giá { get; set; }
        public virtual DbSet<Giỏ_hàng> Giỏ_hàng { get; set; }
        public virtual DbSet<Khách_Hàng> Khách_Hàng { get; set; }
        public virtual DbSet<Sản_phẩm> Sản_phẩm { get; set; }
    }
}
