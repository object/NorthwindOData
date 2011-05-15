using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class ShippersConfiguration : EntityTypeConfiguration<Shippers>
    {
        public ShippersConfiguration(string schemaName)
        {
            ToTable("SHIPPERS", schemaName);

            Property(x => x.ShipperID).HasColumnName("SHIPPER_ID");
            Property(x => x.CompanyName).HasColumnName("COMPANY_NAME");
            Property(x => x.Phone).HasColumnName("PHONE");
        }
    }
}
