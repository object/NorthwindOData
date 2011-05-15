using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class Orders
    {
        [Key]
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual Customers Customer { get; set; }
        public virtual Employees Employee { get; set; }
        [ForeignKey("ShipVia")]
        public virtual Shippers Shipper { get; set; }
    }
}
