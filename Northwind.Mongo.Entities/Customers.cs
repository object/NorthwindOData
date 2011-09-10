using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using MongoDB.Bson;

namespace Northwind.Mongo.Entities
{
    [DataServiceKey("CustomerID")]
    public class Customers
    {
        public ObjectId _id { get; set; }

        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public List<Orders> Orders { get; private set; }
        public List<CustomerDemographics> CustomerDemographics { get; private set; }

        public Customers()
        {
            this.Orders = new List<Orders>();
            this.CustomerDemographics = new List<CustomerDemographics>();
        }
    }
}
