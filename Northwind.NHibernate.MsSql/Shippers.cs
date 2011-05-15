using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("ShipperID")]
    public class Shippers
    {
        public virtual int ShipperID { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string Phone { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }

    public class ShippersMap : ClassMap<Shippers>
    {
        public ShippersMap()
        {
            Id(x => x.ShipperID);
            Map(x => x.CompanyName).Not.Nullable();
            Map(x => x.Phone);
            HasMany(x => x.Orders)
                .Table("Orders")
                .KeyColumn("ShipVia");
        }
    }
}
