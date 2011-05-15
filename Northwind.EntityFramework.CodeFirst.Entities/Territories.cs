using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class Territories
    {
        [Key]
        public string TerritoryID { get; set; }
        [Required]
        public string TerritoryDescription { get; set; }
        public int RegionID { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
        public virtual Regions Region { get; set; }
    }
}
