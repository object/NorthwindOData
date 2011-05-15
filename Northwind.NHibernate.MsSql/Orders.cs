using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("OrderID")]
    public class Orders
    {
        public virtual int OrderID { get; set; }
        public virtual string CustomerID { get; set; }
        public virtual int EmployeeID { get; set; }
        public virtual DateTime? OrderDate { get; set; }
        public virtual DateTime? RequiredDate { get; set; }
        public virtual DateTime? ShippedDate { get; set; }
        public virtual int ShipVia { get; set; }
        public virtual decimal? Freight { get; set; }
        public virtual string ShipName { get; set; }
        public virtual string ShipAddress { get; set; }
        public virtual string ShipCity { get; set; }
        public virtual string ShipRegion { get; set; }
        public virtual string ShipPostalCode { get; set; }
        public virtual string ShipCountry { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual Customers Customer { get; set; }
        public virtual Employees Employee { get; set; }
        public virtual Shippers Shipper { get; set; }
    }

    public class OrdersMap : ClassMap<Orders>
    {
        public OrdersMap()
        {
            Id(x => x.OrderID);
            References(x => x.Customer).Not.LazyLoad().Column("CustomerID");
            References(x => x.Employee).Not.LazyLoad().Column("EmployeeID");
            Map(x => x.OrderDate);
            Map(x => x.RequiredDate);
            Map(x => x.ShippedDate);
            References(x => x.Shipper).Not.LazyLoad().Column("ShipVia");
            Map(x => x.Freight);
            Map(x => x.ShipName);
            Map(x => x.ShipAddress);
            Map(x => x.ShipCity);
            Map(x => x.ShipRegion);
            Map(x => x.ShipPostalCode);
            Map(x => x.ShipCountry);
            HasMany(x => x.OrderDetails)
                .Table("[Order Details]")
                .KeyColumn("OrderID");
        }
    }
}
