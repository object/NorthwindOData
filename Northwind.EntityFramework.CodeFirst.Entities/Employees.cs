using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class Employees
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public AddressDetails AddressDetails { get; private set; }
        public string HomePhone { get; set; }
        public string Extension { get; set; }
        public byte[] Photo { get; set; }
        public string Notes { get; set; }
        public int? ReportsTo { get; set; }
        public string PhotoPath { get; set; }

        [ForeignKey("ReportsTo")]
        public virtual Employees Superior { get; set; }
        public virtual ICollection<Employees> Subordinates { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Territories> Territories { get; set; }

        public Employees()
        {
            this.AddressDetails = new AddressDetails();
        }
    }

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
