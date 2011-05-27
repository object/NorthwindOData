using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.Services;
using System.Data.Services.Providers;
using DataServiceProvider;
using Northwind.Mongo.Service;

namespace Northwind.Mongo.Service
{
    public abstract class MongoDataService : DSPDataService<DSPContext>
    {
        private static DSPContext context;
        private static DSPMetadata metadata;

        /// <summary>Constructor</summary>
        public MongoDataService()
        {
            MongoDataService.context = CreateDataContext();
        }

        public static void ResetDataContext()
        {
            MongoDataService.context = CreateDataContext();
        }

        public static IDisposable RestoreDataContext()
        {
            return new RestoreDataContextDisposable();
        }

        private class RestoreDataContextDisposable : IDisposable
        {
            public void Dispose()
            {
                ResetDataContext();
            }
        }

        private static DSPContext CreateDataContext()
        {
            using (var writer = new StreamWriter(@"D:\Test.txt", true))
            {
                try
                {
                    MongoDataService.metadata = new DSPMetadata("MongoContext", "Northwind");

                    MongoContext mongoContext = new MongoContext(ConfigurationManager.ConnectionStrings["NorthwindContext.MongoDB"].ConnectionString);

                    var itemsType = new ResourceType(typeof (Dictionary<string, object>), 
                        ResourceTypeKind.ComplexType, null, "Northwind", "Items", false);

                    var collectionNames = mongoContext.Database.GetCollectionNames();
                    foreach (var collectionName in collectionNames)
                    {
                        var collectionType = metadata.AddEntityType(collectionName);
                        metadata.AddKeyProperty(collectionType, "ID", typeof(string));
                        metadata.AddComplexProperty(collectionType, "Items", itemsType);
                        metadata.AddResourceSet(collectionName, collectionType);
                    }

                    return new DSPContext();
                }
                catch (Exception exception)
                {
                    writer.WriteLine(exception.Message);
                    writer.WriteLine(exception.StackTrace);
                    throw;
                }
            }
        }

        protected override DSPContext CreateDataSource()
        {
            return MongoDataService.context;
        }

        protected override DSPMetadata CreateDSPMetadata()
        {
            return MongoDataService.metadata;
        }
    }
}
