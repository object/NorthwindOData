using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("CustomerID")]
    public class Customers
    {
        public virtual string CustomerID { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactTitle { get; set; }
        public virtual AddressDetails AddressDetails { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<CustomerDemographics> CustomerDemographics { get; set; }

        public Customers()
        {
            this.AddressDetails = new AddressDetails();
        }
    }

    public class CustomersMap : ClassMap<Customers>
    {
        public CustomersMap()
        {
            Id(x => x.CustomerID);
            Map(x => x.CompanyName).Not.Nullable();
            Map(x => x.ContactName);
            Map(x => x.ContactTitle);
            Component(x => x.AddressDetails);
            Map(x => x.Phone);
            Map(x => x.Fax);
            HasMany(x => x.Orders)
                .Table("Orders")
                .KeyColumn("CustomerID");
            HasManyToMany(x => x.CustomerDemographics)
                .Table("CustomerCustomerDemo")
                .ParentKeyColumn("CustomerID")
                .ChildKeyColumn("CustomerTypeID");
        }
    }
}
