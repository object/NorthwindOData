using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm;
using Norm.Collections;
using Northwind.Utils;

namespace Northwind.MongoDB
{
    public class NorthwindContext : MongoContext
    {
        public NorthwindContext(string connectionString)
            : base(connectionString)
        {
        }

        public IMongoCollection<Categories> Categories { get { return this.GetCollection<Categories>(); } }
        public IMongoCollection<Customers> Customers { get { return this.GetCollection<Customers>(); } }
        public IMongoCollection<CustomerDemographics> CustomerDemographics { get { return this.GetCollection<CustomerDemographics>(); } }
        public IMongoCollection<Employees> Employees { get { return this.GetCollection<Employees>(); } }
        public IMongoCollection<Orders> Orders { get { return this.GetCollection<Orders>(); } }
        public IMongoCollection<OrderDetails> OrderDetails { get { return this.GetCollection<OrderDetails>(); } }
        public IMongoCollection<Products> Products { get { return this.GetCollection<Products>(); } }
        public IMongoCollection<Regions> Regions { get { return this.GetCollection<Regions>(); } }
        public IMongoCollection<Shippers> Shippers { get { return this.GetCollection<Shippers>(); } }
        public IMongoCollection<Suppliers> Suppliers { get { return this.GetCollection<Suppliers>(); } }
        public IMongoCollection<Territories> Territories { get { return this.GetCollection<Territories>(); } }

        public void Populate(object sourceContext)
        {
            using (var mongoAdmin = new MongoAdmin(this.connectionString))
            {
                mongoAdmin.DropDatabase();
            }

            var entityCloner = new EntityCloner(sourceContext, this,
                Add,
                x => GetCollection(x) as IEnumerable<object>,
                () => { });

            entityCloner.CopyEntities();
        }
    }
}
