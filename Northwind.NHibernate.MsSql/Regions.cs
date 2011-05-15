using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("RegionID")]
    public class Regions
    {
        public virtual int RegionID { get; set; }
        public virtual string RegionDescription { get; set; }

        public virtual ICollection<Territories> Territories { get; set; }
    }

    public class RegionsMap : ClassMap<Regions>
    {
        public RegionsMap()
        {
            Table("Region");
            Id(x => x.RegionID);
            Map(x => x.RegionDescription).Not.Nullable();
            HasMany(x => x.Territories)
                .Table("Territories")
                .KeyColumn("RegionID");
        }
    }
}
