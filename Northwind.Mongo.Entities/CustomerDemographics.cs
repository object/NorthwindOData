using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using MongoDB.Bson;

namespace Northwind.Mongo.Entities
{
    [DataServiceKey("CustomerTypeID")]
    public class CustomerDemographics
    {
        public ObjectId _id { get; set; }

        public string CustomerTypeID { get; set; }
        public string CustomerDesc { get; set; }

        public List<Customers> Customers { get; private set; }

        public CustomerDemographics()
        {
            this.Customers = new List<Customers>();
        }
    }
}
