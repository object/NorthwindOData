using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using MongoDB.Bson;

namespace Northwind.Mongo.Entities
{
    [DataServiceKey("RegionID")]
    public class Regions
    {
        public ObjectId _id { get; set; }

        public int RegionID { get; set; }
        public string RegionDescription { get; set; }

        public List<Territories> Territories { get; private set; }

        public Regions()
        {
            this.Territories = new List<Territories>();
        }
    }
}
