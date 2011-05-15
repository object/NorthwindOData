using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("ProductID")]
    public class Products
    {
        public virtual int ProductID { get; set; }
        public virtual string ProductName { get; set; }
        public virtual int SupplierID { get; set; }
        public virtual int CategoryID { get; set; }
        public virtual string QuantityPerUnit { get; set; }
        public virtual decimal? UnitPrice { get; set; }
        public virtual short UnitsInStock { get; set; }
        public virtual short UnitsOnOrder { get; set; }
        public virtual short ReorderLevel { get; set; }
        public virtual bool Discontinued { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Suppliers Supplier { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }

    public class ProductsMap : ClassMap<Products>
    {
        public ProductsMap()
        {
            Id(x => x.ProductID);
            Map(x => x.ProductName).Not.Nullable();
            Map(x => x.QuantityPerUnit);
            Map(x => x.UnitPrice);
            Map(x => x.UnitsInStock);
            Map(x => x.UnitsOnOrder);
            Map(x => x.ReorderLevel);
            Map(x => x.Discontinued).Not.Nullable();
            References(x => x.Category).Not.LazyLoad()
                .Column("CategoryID");
            References(x => x.Supplier).Not.LazyLoad()
                .Column("SupplierID");
            HasMany(x => x.OrderDetails)
                .Table("[Order Details]")
                .KeyColumn("ProductID");
        }
    }
}
