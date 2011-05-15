using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Northwind.EntityFramework.CodeFirst.Entities
{
    public class Customers
    {
        [Key]
        public string CustomerID { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public AddressDetails AddressDetails { get; private set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<CustomerDemographics> CustomerDemographics { get; set; }

        public Customers()
        {
            this.AddressDetails = new AddressDetails();
        }
    }

    public class CustomersConfiguration : EntityTypeConfiguration<Customers>
    {
        public CustomersConfiguration()
        {
            HasMany(x => x.CustomerDemographics)
                .WithMany(x => x.Customers)
                .Map(m => m.MapLeftKey("CustomerID")
                        .MapRightKey("CustomerTypeID")
                        .ToTable("CustomerCustomerDemo"));
        }
    }
}
