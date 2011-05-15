using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.EntityFramework.CodeFirst.MsSql;

namespace Northwind.ModelTests.Query
{
    [TestFixture]
    public class EntityFrameworkCodeFirstMsSqlTests : EntityFrameworkCodeFirstTests<NorthwindContext>
    {
        public override NorthwindContext CreateContext()
        {
            return new NorthwindContext("NorthwindContext.EF.CF.MsSql");
        }
    }
}
