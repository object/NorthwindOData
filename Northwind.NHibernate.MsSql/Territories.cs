using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("TerritoryID")]
    public class Territories
    {
        public virtual string TerritoryID { get; set; }
        public virtual string TerritoryDescription { get; set; }
        public virtual int RegionID { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
        public virtual Regions Region { get; set; }
    }

    public class TerritoriesMap : ClassMap<Territories>
    {
        public TerritoriesMap()
        {
            Id(x => x.TerritoryID);
            Map(x => x.TerritoryDescription);
            References(x => x.Region).Not.LazyLoad().Column("RegionID");
            HasManyToMany(x => x.Employees)
                .Table("EmployeeTerritories")
                .ParentKeyColumn("TerritoryID")
                .ChildKeyColumn("EmployeeID");
        }
    }
}
