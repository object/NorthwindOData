using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class SuppliersConfiguration : EntityTypeConfiguration<Suppliers>
    {
        public SuppliersConfiguration(string schemaName)
        {
            ToTable("SUPPLIERS", schemaName);

            Property(x => x.SupplierID).HasColumnName("SUPPLIER_ID");
            Property(x => x.CompanyName).HasColumnName("COMPANY_NAME");
            Property(x => x.ContactName).HasColumnName("CONTACT_NAME");
            Property(x => x.ContactTitle).HasColumnName("CONTACT_TITLE");
            Property(x => x.Phone).HasColumnName("PHONE");
            Property(x => x.Fax).HasColumnName("FAX");
            Property(x => x.HomePage).HasColumnName("HOME_PAGE");
        }
    }
}
