using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("SupplierID")]
    public class Suppliers
    {
        public virtual int SupplierID { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactTitle { get; set; }
        public virtual AddressDetails AddressDetails { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }
        public virtual string HomePage { get; set; }

        public virtual ICollection<Products> Products { get; set; }

        public Suppliers()
        {
            this.AddressDetails = new AddressDetails();
        }
    }

    public class SuppliersMap : ClassMap<Suppliers>
    {
        public SuppliersMap()
        {
            Id(x => x.SupplierID);
            Map(x => x.CompanyName).Not.Nullable();
            Map(x => x.ContactName);
            Map(x => x.ContactTitle);
            Component(x => x.AddressDetails);
            Map(x => x.Phone);
            Map(x => x.Fax);
            Map(x => x.HomePage);
            HasMany(x => x.Products)
                .Table("Products")
                .KeyColumn("SupplierID");
        }
    }
}
