using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using MongoDB.Bson;

namespace Northwind.Mongo.Entities
{
    [DataServiceKey("ShipperID")]
    public class Shippers
    {
        public ObjectId _id { get; set; }

        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public List<Orders> Orders { get; private set; }

        public Shippers()
        {
            this.Orders = new List<Orders>();
        }
    }
}
