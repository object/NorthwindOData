using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    [Table("Region")]
    public class Regions
    {
        [Key] 
        public int RegionID { get; set; }
        [Required] 
        public string RegionDescription { get; set; }

        public virtual ICollection<Territories> Territories { get; set; }
    }
}
