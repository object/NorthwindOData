using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DataServiceProvider;

namespace Northwind.Mongo.Service
{
    public abstract class MongoMetadataBase : IMongoDSPMetadata
    {
        public DSPMetadata CreateMetadata()
        {
            var metadata = new DSPMetadata("MongoContext", "Northwind");

            using (MongoContext context = new MongoContext(ConfigurationManager.ConnectionStrings["NorthwindContext.MongoDB"].ConnectionString))
            {
                PopulateMetadata(metadata, context);
            }

            return metadata;
        }

        protected abstract void PopulateMetadata(DSPMetadata metadata, MongoContext context);

        protected IEnumerable<string> GetCollectionNames(MongoContext mongoContext)
        {
            return mongoContext.Database.GetCollectionNames().Where(x => !x.StartsWith("system."));
        }
    }
}
