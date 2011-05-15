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
using Northwind.EntityFramework.CodeFirst.MsSql;

namespace Northwind.Service
{
    public class EntityFrameworkCodeFirstMsSql : DataService<ObjectContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        protected override ObjectContext CreateDataSource()
        {
            var ctx = new NorthwindContext("NorthwindContext.EF.CF.MsSql");
            ctx.ObjectContext.ContextOptions.ProxyCreationEnabled = false;
            return ctx.ObjectContext;
        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            base.HandleException(args);
            Elmah.ErrorLog.GetDefault(null).Log(new Error(args.Exception));
        }
    }
}
