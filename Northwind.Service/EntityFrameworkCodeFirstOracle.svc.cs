using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using Elmah;
using Northwind.EntityFramework.CodeFirst.Oracle;

namespace Northwind.Service
{
    public class EntityFrameworkCodeFirstOracle : DataService<ObjectContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        protected override ObjectContext CreateDataSource()
        {
            try
            {
                var ctx = new NorthwindContext("NorthwindContext.EF.CF.Oracle");
                ctx.ObjectContext.ContextOptions.ProxyCreationEnabled = false;
                return ctx.ObjectContext;
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
