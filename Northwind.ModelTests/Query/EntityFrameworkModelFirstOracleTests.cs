using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.EntityFramework.ModelFirst.Oracle;
using Northwind.Utils;

namespace Northwind.ModelTests.Query
{
    [TestFixture]
    public class EntityFrameworkModelFirstOracleTests : QueryTestBase<NorthwindOracleContext,
        Categories, Customers, CustomerDemographics, Employees, Orders, OrderDetails, Products, Regions, Shippers, Suppliers, Territories>
    {
        public override NorthwindOracleContext CreateContext()
        {
            return new NorthwindOracleContext(ConfigurationManager.ConnectionStrings["NorthwindContext.EF.MF.Oracle"].ConnectionString);
        }

        public override void DisposeContext(NorthwindOracleContext context)
        {
            context.Dispose();
        }

        public override IQueryable<Categories> GetCategories()
        {
            return context.Categories;
        }

        public override Categories GetCategory(string categoryName)
        {
            return context.Categories.Where(x => x.CategoryName == categoryName).Single();
        }

        public override IQueryable<Products> GetCategoryProducts(Categories category)
        {
            return category.Products.AsQueryable();
        }

        public override IQueryable<Customers> GetCustomers()
        {
            return context.Customers;
        }

        public override Customers GetCustomer(string customerID)
        {
            return context.Customers.Where(x => x.CustomerID == customerID).Single();
        }

        public override IQueryable<Orders> GetCustomerOrders(Customers customer)
        {
            return customer.Orders.AsQueryable();
        }

        public override IQueryable<CustomerDemographics> GetCustomerCustomerDemographics(Customers customer)
        {
            return customer.CustomerDemographics.AsQueryable();
        }

        public override IQueryable<CustomerDemographics> GetCustomerDemographics()
        {
            return context.CustomerDemographics;
        }

        public override CustomerDemographics GetCustomerDemographics(string customerTypeID)
        {
            return context.CustomerDemographics.Where(x => x.CustomerTypeID == customerTypeID).Single();
        }

        public override IQueryable<Customers> GetCustomerDemographicsCustomers(CustomerDemographics customerDemographics)
        {
            return customerDemographics.Customers.AsQueryable();
        }

        public override IQueryable<Employees> GetEmployees()
        {
            return context.Employees;
        }

        public override Employees GetEmployee(string firstName, string lastName)
        {
            return context.Employees.Where(x => x.FirstName == firstName && x.LastName == lastName).Single();
        }

        public override IQueryable<Employees> GetEmployeeSubordinates(Employees employee)
        {
            return employee.Subordinates.AsQueryable();
        }

        public override Employees GetEmployeeSuperior(Employees employee)
        {
            return employee.Superior;
        }

        public override IQueryable<Orders> GetEmployeeOrders(Employees employee)
        {
            return employee.Orders.AsQueryable();
        }

        public override IQueryable<Territories> GetEmployeeTerritories(Employees employee)
        {
            return employee.Territories.AsQueryable();
        }

        public override IQueryable<Orders> GetOrders()
        {
            return context.Orders;
        }

        public override Orders GetOrder(int orderID)
        {
            return context.Orders.Where(x => x.OrderID == orderID).Single();
        }

        public override IQueryable<OrderDetails> GetOrderOrderDetails(Orders order)
        {
            return order.OrderDetails.AsQueryable();
        }

        public override Customers GetOrderCustomer(Orders order)
        {
            return order.Customer;
        }

        public override Employees GetOrderEmployee(Orders order)
        {
            return order.Employee;
        }

        public override Shippers GetOrderShipper(Orders order)
        {
            return order.Shipper;
        }

        public override IQueryable<OrderDetails> GetOrderDetails()
        {
            return context.OrderDetails;
        }

        public override OrderDetails GetOrderDetails(int orderID, int productID)
        {
            return context.OrderDetails.Where(x => x.OrderID == orderID && x.ProductID == productID).Single();
        }

        public override Orders GetOrderDetailsOrder(OrderDetails orderDetails)
        {
            return orderDetails.Order;
        }

        public override Products GetOrderDetailsProduct(OrderDetails orderDetails)
        {
            return orderDetails.Product;
        }

        public override IQueryable<Products> GetProducts()
        {
            return context.Products;
        }

        public override Products GetProduct(string productName)
        {
            return context.Products.Where(x => x.ProductName == productName).Single();
        }

        public override Categories GetProductCategory(Products product)
        {
            return product.Category;
        }

        public override Suppliers GetProductSupplier(Products product)
        {
            return product.Supplier;
        }

        public override IQueryable<OrderDetails> GetProductOrderDetails(Products product)
        {
            return product.OrderDetails.AsQueryable();
        }

        public override IQueryable<Regions> GetRegions()
        {
            return context.Regions;
        }

        public override Regions GetRegion(string regionDescription)
        {
            return context.Regions.Where(x => x.RegionDescription == regionDescription).Single();
        }

        public override IQueryable<Territories> GetRegionTerritories(Regions region)
        {
            return region.Territories.AsQueryable();
        }

        public override IQueryable<Shippers> GetShippers()
        {
            return context.Shippers;
        }

        public override Shippers GetShipper(string companyName)
        {
            return context.Shippers.Where(x => x.CompanyName == companyName).Single();
        }

        public override IQueryable<Orders> GetShipperOrders(Shippers shipper)
        {
            return shipper.Orders.AsQueryable();
        }

        public override IQueryable<Suppliers> GetSuppliers()
        {
            return context.Suppliers;
        }

        public override Suppliers GetSupplier(string companyName)
        {
            return context.Suppliers.Where(x => x.CompanyName == companyName).Single();
        }

        public override IQueryable<Products> GetSupplierProducts(Suppliers suppliers)
        {
            return suppliers.Products.AsQueryable();
        }

        public override IQueryable<Territories> GetTerritories()
        {
            return context.Territories;
        }

        public override Territories GetTerritory(string territoryDescription)
        {
            return context.Territories.Where(x => x.TerritoryDescription == territoryDescription).Single();
        }

        public override Regions GetTerritoryRegion(Territories territories)
        {
            return territories.Region;
        }

        [Test, Explicit]
        public void CopyData()
        {
            CreateContext();
            var sourceContext = new Northwind.EntityFramework.ModelFirst.MsSql.NorthwindMsSqlContext(ConfigurationManager.ConnectionStrings["NorthwindContext.EF.MF.MsSql"].ConnectionString);

            var entityCloner = new EntityCloner(sourceContext, new OracleEntityCloner(this.context));
            entityCloner.CopyEntities();
        }

        class OracleEntityCloner : ReflectionEntityCloner
        {
            private NorthwindOracleContext context;

            public OracleEntityCloner(NorthwindOracleContext context)
            {
                this.context = context;
            }

            public override object Context { get { return this.context; } }

            public override void AddEntity(object entity, string entityTypeName)
            {
                this.context.AddObject(entityTypeName, entity);
            }

            public override void UpdateEntity(object entity)
            {
            }

            public override void SaveChanges()
            {
                this.context.SaveChanges();
            }
        }
    }
}
