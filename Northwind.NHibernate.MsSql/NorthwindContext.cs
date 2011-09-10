using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Linq;

namespace Northwind.NHibernate.MsSql
{
    public class NorthwindContext : NHibernateDataContext
    {
        public NorthwindContext(ISession session)
            : base(session)
        {
        }

        public IQueryable<Categories> Categories
        {
            get { return new NhQueryable<Categories>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<Customers> Customers
        {
            get { return new NhQueryable<Customers>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<CustomerDemographics> CustomerDemographics
        {
            get { return new NhQueryable<CustomerDemographics>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<Employees> Employees
        {
            get { return new NhQueryable<Employees>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<Orders> Orders
        {
            get { return new NhQueryable<Orders>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<OrderDetails> OrderDetails
        {
            get { return new NhQueryable<OrderDetails>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<Products> Products
        {
            get { return new NhQueryable<Products>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<Regions> Regions
        {
            get { return new NhQueryable<Regions>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<Shippers> Shippers
        {
            get { return new NhQueryable<Shippers>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<Suppliers> Suppliers
        {
            get { return new NhQueryable<Suppliers>(base.Session.As<ISessionImplementor>()); }
        }

        public IQueryable<Territories> Territories
        {
            get { return new NhQueryable<Territories>(base.Session.As<ISessionImplementor>()); }
        }

        public static ISessionFactory CreateSessionFactory(string connectionString)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                .ConnectionString(connectionString)
                .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NorthwindContext>())
                .Cache(c => c
                    .UseQueryCache()
                    .ProviderClass<HashtableCacheProvider>())
                .BuildSessionFactory();
        }

        public new ISession Session
        {
            get { return base.Session; }
        }
    }
}
