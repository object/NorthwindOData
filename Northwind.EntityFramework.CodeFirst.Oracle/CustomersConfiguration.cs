using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class CustomersConfiguration : EntityTypeConfiguration<Customers>
    {
        public CustomersConfiguration(string schemaName)
        {
            ToTable("CUSTOMERS", schemaName);

            Property(x => x.CustomerID).HasColumnName("CUSTOMER_ID");
            Property(x => x.CompanyName).HasColumnName("COMPANY_NAME");
            Property(x => x.ContactName).HasColumnName("CONTACT_NAME");
            Property(x => x.ContactTitle).HasColumnName("CONTACT_TITLE");
            Property(x => x.Phone).HasColumnName("PHONE");
            Property(x => x.Fax).HasColumnName("FAX");

            HasMany(x => x.CustomerDemographics)
                .WithMany(x => x.Customers)
                .Map(m => m.MapLeftKey("CUSTOMER_ID")
                        .MapRightKey("CUSTOMER_TYPE_ID")
                        .ToTable("CUSTOMER_CUSTOMER_DEMO", schemaName));
        }
    }
}
