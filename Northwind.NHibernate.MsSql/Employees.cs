using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("EmployeeID")]
    public class Employees
    {
        public virtual int EmployeeID { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Title { get; set; }
        public virtual string TitleOfCourtesy { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual DateTime? HireDate { get; set; }
        public virtual AddressDetails AddressDetails { get; private set; }
        public virtual string HomePhone { get; set; }
        public virtual string Extension { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual string Notes { get; set; }
        public virtual int? ReportsTo { get; set; }
        public virtual string PhotoPath { get; set; }

        public virtual Employees Superior { get; set; }
        public virtual ICollection<Employees> Subordinates { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Territories> Territories { get; set; }

        public Employees()
        {
            this.AddressDetails = new AddressDetails();
        }
    }

    public class EmployeesMap : ClassMap<Employees>
    {
        public EmployeesMap()
        {
            Id(x => x.EmployeeID);
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.Title);
            Map(x => x.TitleOfCourtesy);
            Map(x => x.BirthDate);
            Map(x => x.HireDate);
            Component(x => x.AddressDetails);
            Map(x => x.HomePhone);
            Map(x => x.Extension);
            Map(x => x.PhotoPath);
            References(x => x.Superior).Not.LazyLoad().Column("ReportsTo");
            Map(x => x.Notes);
            HasMany(x => x.Subordinates)
                .Table("Employee")
                .KeyColumn("ReportsTo");
            HasMany(x => x.Orders)
                .Table("Orders")
                .KeyColumn("EmployeeID");
            HasManyToMany(x => x.Territories)
                .Table("EmployeeTerritories")
                .ParentKeyColumn("EmployeeID")
                .ChildKeyColumn("TerritoryID");
        }
    }
}
