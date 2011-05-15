using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;

namespace Northwind.MongoDB
{
    [DataServiceKey("CustomerTypeID")]
    public class CustomerDemographics
    {
        public string CustomerTypeID { get; set; }
        public string CustomerDesc { get; set; }

        public ICollection<Customers> Customers { get; private set; }

        public CustomerDemographics()
        {
            this.Customers = new List<Customers>();
        }
    }
}
