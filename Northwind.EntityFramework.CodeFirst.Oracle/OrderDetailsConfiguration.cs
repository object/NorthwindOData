using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class OrderDetailsConfiguration : EntityTypeConfiguration<OrderDetails>
    {
        public OrderDetailsConfiguration(string schemaName)
        {
            ToTable("ORDER_DETAILS", schemaName);

            Property(x => x.OrderID).HasColumnName("ORDER_ID");
            Property(x => x.ProductID).HasColumnName("PRODUCT_ID");
            Property(x => x.UnitPrice).HasColumnName("UNIT_PRICE");
            Property(x => x.Quantity).HasColumnName("QUANTITY");
            Property(x => x.Discount).HasColumnName("DISCOUNT");
        }
    }
}
