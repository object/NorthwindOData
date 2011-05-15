using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.MsSql
{
    public class EmployeesConfiguration : EntityTypeConfiguration<Employees>
    {
        public EmployeesConfiguration()
        {
            HasMany(x => x.Territories)
                .WithMany(x => x.Employees)
                .Map(m => m.MapLeftKey("EmployeeID")
                        .MapRightKey("TerritoryID")
                        .ToTable("EmployeeTerritories"));
        }
    }
}
