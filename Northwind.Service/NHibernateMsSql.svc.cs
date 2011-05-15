using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Services;
using System.Data.Services.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using Elmah;
using NHibernate;
using Northwind.NHibernate.MsSql;

namespace Northwind.Service
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class NHibernateMsSql : DataService<NorthwindContext>, IDisposable
    {
        private ISession session;

        public void Dispose()
        {
            if (this.session != null)
            {
                this.session.Dispose();
                this.session = null;
            }
        }

        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.AcceptCountRequests = true;
            config.DataServiceBehavior.AcceptProjectionRequests = true;
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            config.UseVerboseErrors = true;
        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            base.HandleException(args);
            Elmah.ErrorLog.GetDefault(null).Log(new Error(args.Exception));
        }

        protected override NorthwindContext CreateDataSource()
        {
            var factory = NorthwindContext.CreateSessionFactory(
                ConfigurationManager.ConnectionStrings["NorthwindContext.NH.MsSql"].ConnectionString);

            this.session = factory.OpenSession();
            this.session.FlushMode = FlushMode.Auto;

            return new NorthwindContext(this.session);
        }
    }
}
