using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class CustomerDemographics
    {
        [Key]
        public string CustomerTypeID { get; set; }
        public string CustomerDesc { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
    }
}
