using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.MsSql
{
    public class NorthwindContext : NorthwindContextBase
    {
        public NorthwindContext()
            : base()
        {
            Database.SetInitializer<NorthwindContext>(null);
        }

        public NorthwindContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<NorthwindContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AddressDetailsConfiguration());
            modelBuilder.Configurations.Add(new CustomersConfiguration());
            modelBuilder.Configurations.Add(new EmployeesConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
