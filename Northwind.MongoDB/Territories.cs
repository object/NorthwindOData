using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using Norm;

namespace Northwind.MongoDB
{
    [DataServiceKey("TerritoryID")]
    public class Territories
    {
        [MongoIdentifier]
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
