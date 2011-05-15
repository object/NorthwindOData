using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class ProductsConfiguration : EntityTypeConfiguration<Products>
    {
        public ProductsConfiguration(string schemaName)
        {
            ToTable("PRODUCTS", schemaName);

            Property(x => x.ProductID).HasColumnName("PRODUCT_ID");
            Property(x => x.ProductName).HasColumnName("PRODUCT_NAME");
            Property(x => x.SupplierID).HasColumnName("SUPPLIER_ID");
            Property(x => x.CategoryID).HasColumnName("CATEGORY_ID");
            Property(x => x.QuantityPerUnit).HasColumnName("QUANTITY_PER_UNIT");
            Property(x => x.UnitPrice).HasColumnName("UNIT_PRICE");
            Property(x => x.UnitsInStock).HasColumnName("UNITS_IN_STOCK");
            Property(x => x.UnitsOnOrder).HasColumnName("UNITS_ON_ORDER");
            Property(x => x.ReorderLevel).HasColumnName("REORDER_LEVEL");
            Property(x => x.Discontinued).HasColumnName("DISCONTINUED");
        }
    }
}
