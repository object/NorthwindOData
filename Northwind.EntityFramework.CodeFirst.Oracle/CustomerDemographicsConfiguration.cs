using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class CustomerDemographicsConfiguration : EntityTypeConfiguration<CustomerDemographics>
    {
        public CustomerDemographicsConfiguration(string schemaName)
        {
            ToTable("CUSTOMER_DEMOGRAPHICS", schemaName);

            Property(x => x.CustomerTypeID).HasColumnName("CUSTOMER_TYPE_ID");
            Property(x => x.CustomerDesc).HasColumnName("CUSTOMER_DESC");
        }
    }
}
