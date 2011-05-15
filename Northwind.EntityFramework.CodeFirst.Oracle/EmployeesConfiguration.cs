using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class EmployeesConfiguration : EntityTypeConfiguration<Employees>
    {
        public EmployeesConfiguration(string schemaName)
        {
            ToTable("EMPLOYEES", schemaName);

            Property(x => x.EmployeeID).HasColumnName("EMPLOYEE_ID");
            Property(x => x.LastName).HasColumnName("LAST_NAME");
            Property(x => x.FirstName).HasColumnName("FIRST_NAME");
            Property(x => x.Title).HasColumnName("TITLE");
            Property(x => x.TitleOfCourtesy).HasColumnName("TITLE_OF_COURTESY");
            Property(x => x.BirthDate).HasColumnName("BIRTH_DATE");
            Property(x => x.HireDate).HasColumnName("HIRE_DATE");
            Property(x => x.HomePhone).HasColumnName("HOME_PHONE");
            Property(x => x.Extension).HasColumnName("EXTENSION");
            Property(x => x.Photo).HasColumnName("PHOTO");
            Property(x => x.Notes).HasColumnName("NOTES");
            Property(x => x.ReportsTo).HasColumnName("REPORTS_TO");
            Property(x => x.PhotoPath).HasColumnName("PHOTO_PATH");

            HasMany(x => x.Territories)
                .WithMany(x => x.Employees)
                .Map(m => m.MapLeftKey("EMPLOYEE_ID")
                        .MapRightKey("TERRITORY_ID")
                        .ToTable("EMPLOYEE_TERRITORIES", schemaName));
        }
    }
}
