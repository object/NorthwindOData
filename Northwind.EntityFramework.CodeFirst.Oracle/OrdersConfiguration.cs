using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class OrdersConfiguration : EntityTypeConfiguration<Orders>
    {
        public OrdersConfiguration(string schemaName)
        {
            ToTable("ORDERS", schemaName);

            Property(x => x.OrderID).HasColumnName("ORDER_ID");
            Property(x => x.CustomerID).HasColumnName("CUSTOMER_ID");
            Property(x => x.EmployeeID).HasColumnName("EMPLOYEE_ID");
            Property(x => x.OrderDate).HasColumnName("ORDER_DATE");
            Property(x => x.RequiredDate).HasColumnName("REQUIRED_DATE");
            Property(x => x.ShippedDate).HasColumnName("SHIPPED_DATE");
            Property(x => x.ShipVia).HasColumnName("SHIP_VIA");
            Property(x => x.Freight).HasColumnName("FREIGHT");
            Property(x => x.ShipName).HasColumnName("SHIP_NAME");
            Property(x => x.ShipAddress).HasColumnName("SHIP_ADDRESS");
            Property(x => x.ShipCity).HasColumnName("SHIP_CITY");
            Property(x => x.ShipRegion).HasColumnName("SHIP_REGION");
            Property(x => x.ShipPostalCode).HasColumnName("SHIP_POSTAL_CODE");
            Property(x => x.ShipCountry).HasColumnName("SHIP_COUNTRY");
        }
    }
}
