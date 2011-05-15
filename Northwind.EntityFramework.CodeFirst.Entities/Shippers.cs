using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class Shippers
    {
        [Key] 
        public int ShipperID { get; set; }
        [Required] 
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
