﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineFoodOrderingSystem.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using WebApplication1.DAL;

    public partial class Food_OrderingEntities : DbContext
    {
        public Food_OrderingEntities()
            : base("name=Food_OrderingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Tbl_MenuCategory> Tbl_MenuCategory { get; set; }
        public virtual DbSet<tbl_MenuItem> tbl_MenuItem { get; set; }
        public virtual DbSet<Tbl_Member> Tbl_Member { get; set; }

        public System.Data.Entity.DbSet<OnlineFoodOrderingSystem.Models.FoodCategory> FoodCategories { get; set; }

        public System.Data.Entity.DbSet<OnlineFoodOrderingSystem.Models.Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<OnlineFoodOrderingSystem.Models.Menu> Menus { get; set; }

        public System.Data.Entity.DbSet<OnlineFoodOrderingSystem.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<OnlineFoodOrderingSystem.Models.EmployeeOrder> EmployeeOrders { get; set; }

        public System.Data.Entity.DbSet<OnlineFoodOrderingSystem.Models.OrderedFoood> OrderedFooods { get; set; }
    }
}