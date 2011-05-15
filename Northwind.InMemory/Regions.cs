using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;

namespace Northwind.InMemory
{
    [DataServiceKey("RegionID")]
    public class Regions
    {
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }

        public ICollection<Territories> Territories { get; private set; }

        public Regions()
        {
            this.Territories = new List<Territories>();
        }
    }
}
