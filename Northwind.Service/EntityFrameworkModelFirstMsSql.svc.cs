using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using Northwind.EntityFramework.ModelFirst.MsSql;
using Elmah;

namespace Northwind.Service
{
    public class EntityFrameworkModelFirstMsSql : DataService<NorthwindMsSqlContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        protected override NorthwindMsSqlContext CreateDataSource()
        {
            return new NorthwindMsSqlContext(ConfigurationManager.ConnectionStrings["NorthwindContext.EF.MF.MsSql"].ConnectionString);
        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            base.HandleException(args);
            Elmah.ErrorLog.GetDefault(null).Log(new Error(args.Exception));
        }
    }
}
