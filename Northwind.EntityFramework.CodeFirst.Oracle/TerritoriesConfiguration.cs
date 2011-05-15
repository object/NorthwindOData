using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class TerritoriesConfiguration : EntityTypeConfiguration<Territories>
    {
        public TerritoriesConfiguration(string schemaName)
        {
            ToTable("TERRITORIES", schemaName);

            Property(x => x.TerritoryID).HasColumnName("TERRITORY_ID");
            Property(x => x.TerritoryDescription).HasColumnName("TERRITORY_DESCRIPTION");
            Property(x => x.RegionID).HasColumnName("REGION_ID");
        }
    }
}
