using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Northwind.Utils;
using NUnit.Framework;
using Northwind.MongoDB;

namespace Northwind.ModelTests.Query
{
    [TestFixture]
    public class MongoDbTests
    {
        internal MongoContext context;

        public MongoContext CreateContext()
        {
            this.context = new MongoContext(ConfigurationManager.ConnectionStrings["NorthwindContext.MongoDB"].ConnectionString);
            return this.context;
        }

        [Test, Explicit]
        public void CopyData()
        {
            var sourceContext = new Northwind.EntityFramework.ModelFirst.MsSql.NorthwindMsSqlContext(ConfigurationManager.ConnectionStrings["NorthwindContext.EF.MF.MsSql"].ConnectionString);
            var destinationContext = new NorthwindContext(ConfigurationManager.ConnectionStrings["NorthwindContext.MongoDB"].ConnectionString);

            destinationContext.Populate(sourceContext);
        }
    }
}
