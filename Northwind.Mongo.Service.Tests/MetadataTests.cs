using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.Mongo.Service;

namespace Northwind.Mongo.Service.Tests
{
    [TestFixture]
    public class MetadataTests
    {
        [Test]
        public void should_create_key_value_context()
        {
            var context = MongoDataService<MongoKeyValueContext, MongoKeyValueMetadata>.CreateDataContext();
            Assert.IsNotNull(context);
        }

        [Test]
        public void should_create_strong_type_context()
        {
            var context = MongoDataService<MongoStrongTypeContext, MongoStrongTypeMetadata>.CreateDataContext();
            Assert.IsNotNull(context);
        }
    }
}
