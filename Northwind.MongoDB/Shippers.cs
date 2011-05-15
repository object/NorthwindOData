using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;

namespace Northwind.MongoDB
{
    [DataServiceKey("ShipperID")]
    public class Shippers
    {
        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public ICollection<Orders> Orders { get; private set; }

        public Shippers()
        {
            this.Orders = new List<Orders>();
        }
    }
}
