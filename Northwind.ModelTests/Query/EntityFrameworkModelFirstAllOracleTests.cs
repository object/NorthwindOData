using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Northwind.EntityFramework.ModelFirst;
using NUnit.Framework;

namespace Northwind.ModelTests.Query
{
    [TestFixture]
    public class EntityFrameworkModelFirstAllOracleTests : EntityFrameworkModelFirstAllTests
    {
        public override NorthwindContext CreateContext()
        {
            return new NorthwindContext(ConfigurationManager.ConnectionStrings["NorthwindContext.EF.MF.All.Oracle"].ConnectionString);
        }
    }
}
