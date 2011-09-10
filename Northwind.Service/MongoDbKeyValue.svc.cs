using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using DataServiceProvider;
using Elmah;
using Northwind.Mongo.Service;

namespace Northwind.Service
{
    public class MongoDbKeyValue : MongoDataService<MongoKeyValueContext, MongoKeyValueMetadata>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            config.DataServiceBehavior.AcceptCountRequests = true;
            config.DataServiceBehavior.AcceptProjectionRequests = true;
            config.UseVerboseErrors = true;
        }

        protected override DSPContext CreateDataSource()
        {
            try
            {
                return base.CreateDataSource();
            }
            catch (Exception exception)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Error(exception));
                throw;
            }
        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            base.HandleException(args);
            Elmah.ErrorLog.GetDefault(null).Log(new Error(args.Exception));
        }
    }
}
