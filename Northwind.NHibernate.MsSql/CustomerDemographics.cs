using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("CustomerTypeID")]
    public class CustomerDemographics
    {
        public virtual string CustomerTypeID { get; set; }
        public virtual string CustomerDesc { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
    }

    public class CustomerDemographicsMap : ClassMap<CustomerDemographics>
    {
        public CustomerDemographicsMap()
        {
            Id(x => x.CustomerTypeID);
            Map(x => x.CustomerDesc);
            HasManyToMany(x => x.Customers)
                .Table("CustomerCustomerDemo")
                .ParentKeyColumn("CustomerTypeID")
                .ChildKeyColumn("CustomerID");
        }
    }
}
