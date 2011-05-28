using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.Services;
using System.Data.Services.Providers;
using DataServiceProvider;
using MongoDB.Bson;
using Northwind.Mongo.Service;

namespace Northwind.Mongo.Service
{
    public abstract class MongoDataService<TDSPContext, TDSPMetadata> : DSPDataService<DSPContext>
        where TDSPContext : IMongoDSPContext, new()
        where TDSPMetadata : IMongoDSPMetadata, new()
    {
        private static DSPContext context;
        private static DSPMetadata metadata;

        /// <summary>Constructor</summary>
        public MongoDataService()
        {
            context = CreateDataContext();
        }

        public static void ResetDataContext()
        {
            context = CreateDataContext();
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

        protected override DSPContext CreateDataSource()
        {
            return context;
        }

        protected override DSPMetadata CreateDSPMetadata()
        {
            return metadata;
        }

        internal static DSPContext CreateDataContext()
        {
            metadata = new TDSPMetadata().CreateMetadata();
            return new TDSPContext().CreateContext(metadata);
        }
    }
}
