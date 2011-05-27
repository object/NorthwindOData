using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Northwind.Utils;

namespace Northwind.InMemory
{
    public sealed partial class NorthwindContext
    {
        static readonly NorthwindContext instance = new NorthwindContext();

        static NorthwindContext()
        {
        }

        NorthwindContext()
        {
            Populate();
        }

        public static NorthwindContext Instance
        {
            get
            {
                return instance;
            }
        }

        private ICollection<Categories> categories = new List<Categories>();
        private ICollection<Customers> customers = new List<Customers>();
        private ICollection<CustomerDemographics> customerDemographics = new List<CustomerDemographics>();
        private ICollection<Employees> employees = new List<Employees>();
        private ICollection<Orders> orders = new List<Orders>();
        private ICollection<OrderDetails> orderDetails = new List<OrderDetails>();
        private ICollection<Products> products = new List<Products>();
        private ICollection<Regions> regions = new List<Regions>();
        private ICollection<Shippers> shippers = new List<Shippers>();
        private ICollection<Suppliers> suppliers = new List<Suppliers>();
        private ICollection<Territories> territories = new List<Territories>();

        public IQueryable<Categories> Categories { get { return this.categories.AsQueryable(); } }
        public IQueryable<Customers> Customers { get { return this.customers.AsQueryable(); } }
        public IQueryable<CustomerDemographics> CustomerDemographics { get { return this.customerDemographics.AsQueryable(); } }
        public IQueryable<Employees> Employees { get { return this.employees.AsQueryable(); } }
        public IQueryable<Orders> Orders { get { return this.orders.AsQueryable(); } }
        public IQueryable<OrderDetails> OrderDetails { get { return this.orderDetails.AsQueryable(); } }
        public IQueryable<Products> Products { get { return this.products.AsQueryable(); } }
        public IQueryable<Regions> Regions { get { return this.regions.AsQueryable(); } }
        public IQueryable<Shippers> Shippers { get { return this.shippers.AsQueryable(); } }
        public IQueryable<Suppliers> Suppliers { get { return this.suppliers.AsQueryable(); } }
        public IQueryable<Territories> Territories { get { return this.territories.AsQueryable(); } }

        private void Populate()
        {
            var sourceContext = new Northwind.EntityFramework.ModelFirst.MsSql.NorthwindMsSqlContext(ConfigurationManager.ConnectionStrings["NorthwindContext.EF.MF.MsSql"].ConnectionString);

            var entityCloner = new EntityCloner(sourceContext, new InMemoryEntityCloner(this));
            entityCloner.CopyEntities();
        }
    }

    class InMemoryEntityCloner : ReflectionEntityCloner
    {
        private NorthwindContext context;
        private Dictionary<string, Type> entityTypes = new Dictionary<string, Type>();

        public InMemoryEntityCloner(NorthwindContext context)
        {
            this.context = context;
        }

        public override object Context { get { return this.context; } }

        public override void AddEntity(object entity, string entityTypeName)
        {
            this.context.AddEntityToContainer(entity);
        }

        public override void UpdateEntity(object entity)
        {
        }

        public override void SaveChanges()
        {
        }
    }
}
