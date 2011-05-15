using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class CategoriesConfiguration : EntityTypeConfiguration<Categories>
    {
        public CategoriesConfiguration(string schemaName)
        {
            ToTable("CATEGORIES", schemaName);

            Property(x => x.CategoryID).HasColumnName("CATEGORY_ID");
            Property(x => x.CategoryName).HasColumnName("CATEGORY_NAME");
            Property(x => x.Description).HasColumnName("DESCRIPTION");
            Property(x => x.Picture).HasColumnName("PICTURE");
        }
    }
}
