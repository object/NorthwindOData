using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class AddressDetailsConfiguration : ComplexTypeConfiguration<AddressDetails>
    {
        public AddressDetailsConfiguration()
        {
            Property(x => x.Address).HasColumnName("ADDRESS");
            Property(x => x.City).HasColumnName("CITY");
            Property(x => x.Region).HasColumnName("REGION");
            Property(x => x.PostalCode).HasColumnName("POSTAL_CODE");
            Property(x => x.Country).HasColumnName("COUNTRY");
        }
    }
}
