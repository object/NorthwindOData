using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.MsSql
{
    public class AddressDetailsConfiguration : ComplexTypeConfiguration<AddressDetails>
    {
        public AddressDetailsConfiguration()
        {
            Property(x => x.Address).HasColumnName("Address");
            Property(x => x.City).HasColumnName("City");
            Property(x => x.Region).HasColumnName("Region");
            Property(x => x.PostalCode).HasColumnName("PostalCode");
            Property(x => x.Country).HasColumnName("Country");
        }
    }
}
