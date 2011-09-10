using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using MongoDB.Bson;

namespace Northwind.Mongo.Entities
{
    [DataServiceKey("TerritoryID")]
    public class Territories
    {
        public ObjectId _id { get; set; }

        public string TerritoryID { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionID { get; set; }

        public List<Employees> Employees { get; private set; }
        public Regions Region { get; set; }

        public Territories()
        {
            this.Employees = new List<Employees>();
        }
    }
}
