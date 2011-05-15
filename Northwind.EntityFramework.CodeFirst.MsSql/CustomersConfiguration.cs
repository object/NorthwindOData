using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.MsSql
{
    public class CustomersConfiguration : EntityTypeConfiguration<Customers>
    {
        public CustomersConfiguration()
        {
            HasMany(x => x.CustomerDemographics)
                .WithMany(x => x.Customers)
                .Map(m => m.MapLeftKey("CustomerID")
                        .MapRightKey("CustomerTypeID")
                        .ToTable("CustomerCustomerDemo"));
        }
    }
}
