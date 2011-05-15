using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class RegionsConfiguration : EntityTypeConfiguration<Regions>
    {
        public RegionsConfiguration(string schemaName)
        {
            ToTable("REGION", schemaName);

            Property(x => x.RegionID).HasColumnName("REGION_ID");
            Property(x => x.RegionDescription).HasColumnName("REGION_DESCRIPTION");
        }
    }
}
