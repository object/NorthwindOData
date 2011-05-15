using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey(new string[] { "OrderID", "ProductID" })]
    public class OrderDetails
    {
        public virtual int OrderID { get; set; }
        public virtual int ProductID { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual short Quantity { get; set; }
        public virtual float Discount { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode = hashCode ^ OrderID.GetHashCode() ^ ProductID.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            var objectToCompare = obj as OrderDetails;
            if (objectToCompare == null)
            {
                return false;
            }
            return (this.GetHashCode() != objectToCompare.GetHashCode());
        }
    }

    public class OrderDetailsMap : ClassMap<OrderDetails>
    {
        public OrderDetailsMap()
        {
            Table("[Order Details]");
            CompositeId()
                .KeyProperty(x => x.OrderID)
                .KeyProperty(x => x.ProductID);
            References(x => x.Order).Not.LazyLoad().Column("OrderID");
            References(x => x.Product).Not.LazyLoad().Column("ProductID");
            Map(x => x.UnitPrice).Not.Nullable();
            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.Discount).Not.Nullable();
        }
    }
}
