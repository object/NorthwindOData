using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class Categories
    {
        [Key] 
        public int CategoryID { get; set; }
        [Required] 
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
