using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using NUnit.Framework;
using Northwind.Mongo.Entities;

namespace Northwind.ModelTests.Query
{
    [TestFixture]
    public class MongoDbTests : QueryTestBase<NorthwindContext,
        Categories, Customers, CustomerDemographics, Employees, Orders, OrderDetails, Products, Regions, Shippers, Suppliers, Territories>
    {
        public override NorthwindContext CreateContext()
        {
            this.context = new NorthwindContext(ConfigurationManager.ConnectionStrings["NorthwindContext.MongoDB"].ConnectionString);
            return this.context;
        }

        public override void DisposeContext(NorthwindContext context)
        {
            context.Dispose();
        }

        public override IQueryable<Categories> GetCategories()
        {
            return this.context.Categories.FindAll().AsQueryable();
        }

        public override Categories GetCategory(string categoryName)
        {
            return context.Categories.FindOne(MongoDB.Driver.Builders.Query.EQ("CategoryName", categoryName));
        }

        public override IQueryable<Products> GetCategoryProducts(Categories category)
        {
            return category.Products.AsQueryable();
        }

        public override IQueryable<Customers> GetCustomers()
        {
            return this.context.Customers.FindAll().AsQueryable();
        }

        public override Customers GetCustomer(string customerID)
        {
            return context.Customers.FindOne(MongoDB.Driver.Builders.Query.EQ("CustomerID", customerID));
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
            return context.CustomerDemographics.FindAll().AsQueryable();
        }

        public override CustomerDemographics GetCustomerDemographics(string customerTypeID)
        {
            return context.CustomerDemographics.FindOne(MongoDB.Driver.Builders.Query.EQ("CustomerTypeID", customerTypeID));
        }

        public override IQueryable<Customers> GetCustomerDemographicsCustomers(CustomerDemographics customerDemographics)
        {
            return customerDemographics.Customers.AsQueryable();
        }

        public override IQueryable<Employees> GetEmployees()
        {
            return this.context.Employees.FindAll().AsQueryable();
        }

        public override Employees GetEmployee(string firstName, string lastName)
        {
            return context.Employees.FindOne(MongoDB.Driver.Builders.Query.And(
                MongoDB.Driver.Builders.Query.EQ("FirstName", firstName),
                MongoDB.Driver.Builders.Query.EQ("LastName", lastName)));
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
            return this.context.Orders.FindAll().AsQueryable();
        }

        public override Orders GetOrder(int orderID)
        {
            return context.Orders.FindOne(MongoDB.Driver.Builders.Query.EQ("OrderID", orderID));
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
            return this.context.OrderDetails.FindAll().AsQueryable();
        }

        public override OrderDetails GetOrderDetails(int orderID, int productID)
        {
            return context.OrderDetails.FindOne(MongoDB.Driver.Builders.Query.And(
                MongoDB.Driver.Builders.Query.EQ("OrderID", orderID),
                MongoDB.Driver.Builders.Query.EQ("ProductID", productID)));
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
            return this.context.Products.FindAll().AsQueryable();
        }

        public override Products GetProduct(string productName)
        {
            return context.Products.FindOne(MongoDB.Driver.Builders.Query.EQ("ProductName", productName));
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
            return this.context.Regions.FindAll().AsQueryable();
        }

        public override Regions GetRegion(string regionDescription)
        {
            BsonRegularExpression regex = new BsonRegularExpression(string.Format("{0}(\\s+)", regionDescription));
            return context.Regions.FindOne(MongoDB.Driver.Builders.Query.Matches("RegionDescription", regex));
        }

        public override IQueryable<Territories> GetRegionTerritories(Regions region)
        {
            return region.Territories.AsQueryable();
        }

        public override IQueryable<Shippers> GetShippers()
        {
            return this.context.Shippers.FindAll().AsQueryable();
        }

        public override Shippers GetShipper(string companyName)
        {
            return context.Shippers.FindOne(MongoDB.Driver.Builders.Query.EQ("CompanyName", companyName));
        }

        public override IQueryable<Orders> GetShipperOrders(Shippers shipper)
        {
            return shipper.Orders.AsQueryable();
        }

        public override IQueryable<Suppliers> GetSuppliers()
        {
            return this.context.Suppliers.FindAll().AsQueryable();
        }

        public override Suppliers GetSupplier(string companyName)
        {
            return context.Suppliers.FindOne(MongoDB.Driver.Builders.Query.EQ("CompanyName", companyName));
        }

        public override IQueryable<Products> GetSupplierProducts(Suppliers suppliers)
        {
            return suppliers.Products.AsQueryable();
        }

        public override IQueryable<Territories> GetTerritories()
        {
            return this.context.Territories.FindAll().AsQueryable();
        }

        public override Territories GetTerritory(string territoryDescription)
        {
            BsonRegularExpression regex = new BsonRegularExpression(string.Format("{0}(\\s+)", territoryDescription));
            return context.Territories.FindOne(MongoDB.Driver.Builders.Query.Matches("TerritoryDescription", territoryDescription));
        }

        public override Regions GetTerritoryRegion(Territories territories)
        {
            return territories.Region;
        }

        [Test, Explicit]
        public void CopyData()
        {
            var sourceContext = new Northwind.EntityFramework.ModelFirst.MsSql.NorthwindMsSqlContext(ConfigurationManager.ConnectionStrings["NorthwindContext.EF.MF.MsSql"].ConnectionString);
            var destinationContext = new NorthwindContext(ConfigurationManager.ConnectionStrings["NorthwindContext.MongoDB"].ConnectionString);

            destinationContext.Populate(sourceContext);
        }
    }
}
