using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations;
using Northwind.EntityFramework.CodeFirst.Entities;

namespace Northwind.EntityFramework.CodeFirst.Oracle
{
    public class NorthwindContext : NorthwindContextBase
    {
        public NorthwindContext()
            : base()
        {
            Database.SetInitializer<NorthwindContext>(null);
        }

        public NorthwindContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<NorthwindContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ColumnTypeCasingConvention>();
            string schemaName = GetSchemaName();
            modelBuilder.Entity<EdmMetadata>().ToTable("EdmMetadata", schemaName);

            modelBuilder.Configurations.Add(new AddressDetailsConfiguration());
            modelBuilder.Configurations.Add(new CategoriesConfiguration(schemaName));
            modelBuilder.Configurations.Add(new CustomerDemographicsConfiguration(schemaName));
            modelBuilder.Configurations.Add(new CustomersConfiguration(schemaName));
            modelBuilder.Configurations.Add(new EmployeesConfiguration(schemaName));
            modelBuilder.Configurations.Add(new OrderDetailsConfiguration(schemaName));
            modelBuilder.Configurations.Add(new OrdersConfiguration(schemaName));
            modelBuilder.Configurations.Add(new ProductsConfiguration(schemaName));
            modelBuilder.Configurations.Add(new RegionsConfiguration(schemaName));
            modelBuilder.Configurations.Add(new ShippersConfiguration(schemaName));
            modelBuilder.Configurations.Add(new SuppliersConfiguration(schemaName));
            modelBuilder.Configurations.Add(new TerritoriesConfiguration(schemaName));

            base.OnModelCreating(modelBuilder);
        }

        private string GetSchemaName()
        {
            foreach (var item in Database.Connection.ConnectionString.Split(';'))
            {
                if (item.Trim().ToLower().StartsWith("user id"))
                {
                    return item.Split('=')[1].Trim().ToUpper();
                }
            }
            return null;
        }
    }
}
