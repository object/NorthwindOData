using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class NorthwindContextBase : DbContext
    {
        public NorthwindContextBase()
            : base()
        {
        }

        public NorthwindContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public ObjectContext ObjectContext
        {
            get { return (this as IObjectContextAdapter).ObjectContext; }
        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<CustomerDemographics> CustomerDemographics { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Regions> Regions { get; set; }
        public DbSet<Shippers> Shippers { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Territories> Territories { get; set; }
    }
}
