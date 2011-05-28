using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataServiceProvider;

namespace Northwind.Mongo.Service
{
    public class MongoKeyValueContext : IMongoDSPContext
    {
        public DSPContext CreateContext(DSPMetadata metadata)
        {
            return new DSPContext();
        }
    }
}
