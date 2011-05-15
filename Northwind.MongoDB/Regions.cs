using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using Norm;

namespace Northwind.MongoDB
{
    [DataServiceKey("RegionID")]
    public class Regions
    {
        [MongoIdentifier]
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }

        public List<Territories> Territories { get; private set; }

        public Regions()
        {
            this.Territories = new List<Territories>();
        }
    }
}
