using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class Suppliers
    {
        [Key] 
        public int SupplierID { get; set; }
        [Required] 
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public AddressDetails AddressDetails { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string HomePage { get; set; }

        public virtual ICollection<Products> Products { get; set; }

        public Suppliers()
        {
            this.AddressDetails = new AddressDetails();
        }
    }
}
