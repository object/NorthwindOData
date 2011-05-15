using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.ServiceTests;
using Northwind.ServiceTests.EntityFrameworkModelFirstMsSqlService;

namespace Northwind.ServiceTests.Query
{
    [TestFixture]
    public class EntityFrameworkModelFirstMsSqlTests : QueryTestBase<NorthwindMsSqlContext,
        Categories, Customers, CustomerDemographics, Employees, Orders, OrderDetails, Products, Regions, Shippers, Suppliers, Territories>
    {
        public override NorthwindMsSqlContext CreateContext()
        {
            return new NorthwindMsSqlContext(new Uri("http://localhost.:50555/EntityFrameworkModelFirstMsSql.svc/"));
        }

        public override DataServiceQuery<Categories> GetCategories()
        {
            return context.Categories;
        }

        public override Categories GetCategory(string categoryName)
        {
            return context.Categories.Where(x => x.CategoryName == categoryName).Single();
        }

        public override IEnumerable<Products> GetCategoryProducts(Categories category)
        {
            return context.Categories.Where(x => x.CategoryID == category.CategoryID).SelectMany(x => x.Products);
        }

        public override DataServiceQuery<Customers> GetCustomers()
        {
            return context.Customers;
        }

        public override Customers GetCustomer(string customerID)
        {
            return context.Customers.Where(x => x.CustomerID == customerID).Single();
        }

        public override IEnumerable<Orders> GetCustomerOrders(Customers customer)
        {
            return context.Customers.Where(x => x.CustomerID == customer.CustomerID).SelectMany(x => x.Orders);
        }

        public override IEnumerable<CustomerDemographics> GetCustomerCustomerDemographics(Customers customer)
        {
            return context.Customers.Where(x => x.CustomerID == customer.CustomerID).SelectMany(x => x.CustomerDemographics);
        }

        public override DataServiceQuery<CustomerDemographics> GetCustomerDemographics()
        {
            return context.CustomerDemographics;
        }

        public override CustomerDemographics GetCustomerDemographics(string customerTypeID)
        {
            return context.CustomerDemographics.Where(x => x.CustomerTypeID == customerTypeID).Single();
        }

        public override IEnumerable<Customers> GetCustomerDemographicsCustomers(CustomerDemographics customerDemographics)
        {
            return context.CustomerDemographics.Where(x => x.CustomerTypeID == customerDemographics.CustomerTypeID).SelectMany(x => x.Customers);
        }

        public override DataServiceQuery<Employees> GetEmployees()
        {
            return context.Employees;
        }

        public override Employees GetEmployee(string firstName, string lastName)
        {
            return context.Employees.Where(x => x.FirstName == firstName && x.LastName == lastName).Single();
        }

        public override IEnumerable<Employees> GetEmployeeSubordinates(Employees employee)
        {
            return context.Employees.Where(x => x.EmployeeID == employee.EmployeeID).SelectMany(x => x.Subordinates);
        }

        public override Employees GetEmployeeSuperior(Employees employee)
        {
            return context.Employees.Where(x => x.EmployeeID == employee.EmployeeID).Select(x => x.Superior).Single();
        }

        public override IEnumerable<Orders> GetEmployeeOrders(Employees employee)
        {
            return context.Employees.Where(x => x.EmployeeID == employee.EmployeeID).SelectMany(x => x.Orders);
        }

        public override IEnumerable<Territories> GetEmployeeTerritories(Employees employee)
        {
            return context.Employees.Where(x => x.EmployeeID == employee.EmployeeID).SelectMany(x => x.Territories);
        }

        public override DataServiceQuery<Orders> GetOrders()
        {
            return context.Orders;
        }

        public override Orders GetOrder(int orderID)
        {
            return context.Orders.Where(x => x.OrderID == orderID).Single();
        }

        public override IEnumerable<OrderDetails> GetOrderOrderDetails(Orders order)
        {
            return context.Orders.Where(x => x.OrderID == order.OrderID).SelectMany(x => x.OrderDetails);
        }

        public override Customers GetOrderCustomer(Orders order)
        {
            return context.Orders.Where(x => x.OrderID == order.OrderID).Select(x => x.Customer).Single();
        }

        public override Employees GetOrderEmployee(Orders order)
        {
            return context.Orders.Where(x => x.OrderID == order.OrderID).Select(x => x.Employee).Single();
        }

        public override Shippers GetOrderShipper(Orders order)
        {
            return context.Orders.Where(x => x.OrderID == order.OrderID).Select(x => x.Shipper).Single();
        }

        public override DataServiceQuery<OrderDetails> GetOrderDetails()
        {
            return context.OrderDetails;
        }

        public override OrderDetails GetOrderDetails(int orderID, int productID)
        {
            return context.OrderDetails.Where(x => x.OrderID == orderID && x.ProductID == productID).Single();
        }

        public override Orders GetOrderDetailsOrder(OrderDetails orderDetails)
        {
            return context.OrderDetails.Where(x => x.OrderID == orderDetails.OrderID && x.ProductID == orderDetails.ProductID).Select(x => x.Order).Single();
        }

        public override Products GetOrderDetailsProduct(OrderDetails orderDetails)
        {
            return context.OrderDetails.Where(x => x.OrderID == orderDetails.OrderID && x.ProductID == orderDetails.ProductID).Select(x => x.Product).Single();
        }

        public override DataServiceQuery<Products> GetProducts()
        {
            return context.Products;
        }

        public override Products GetProduct(string productName)
        {
            return context.Products.Where(x => x.ProductName == productName).Single();
        }

        public override Categories GetProductCategory(Products product)
        {
            return context.Products.Where(x => x.ProductID == product.ProductID).Select(x => x.Category).Single();
        }

        public override Suppliers GetProductSupplier(Products product)
        {
            return context.Products.Where(x => x.ProductID == product.ProductID).Select(x => x.Supplier).Single();
        }

        public override IEnumerable<OrderDetails> GetProductOrderDetails(Products product)
        {
            return context.Products.Where(x => x.ProductID == product.ProductID).SelectMany(x => x.OrderDetails);
        }

        public override DataServiceQuery<Regions> GetRegions()
        {
            return context.Regions;
        }

        public override Regions GetRegion(string regionDescription)
        {
            return context.Regions.Where(x => x.RegionDescription.Trim() == regionDescription).Single();
        }

        public override IEnumerable<Territories> GetRegionTerritories(Regions region)
        {
            return context.Regions.Where(x => x.RegionID == region.RegionID).SelectMany(x => x.Territories);
        }

        public override DataServiceQuery<Shippers> GetShippers()
        {
            return context.Shippers;
        }

        public override Shippers GetShipper(string companyName)
        {
            return context.Shippers.Where(x => x.CompanyName == companyName).Single();
        }

        public override IEnumerable<Orders> GetShipperOrders(Shippers shipper)
        {
            return context.Shippers.Where(x => x.ShipperID == shipper.ShipperID).SelectMany(x => x.Orders);
        }

        public override DataServiceQuery<Suppliers> GetSuppliers()
        {
            return context.Suppliers;
        }

        public override Suppliers GetSupplier(string companyName)
        {
            return context.Suppliers.Where(x => x.CompanyName == companyName).Single();
        }

        public override IEnumerable<Products> GetSupplierProducts(Suppliers suppliers)
        {
            return context.Suppliers.Where(x => x.SupplierID == suppliers.SupplierID).SelectMany(x => x.Products);
        }

        public override DataServiceQuery<Territories> GetTerritories()
        {
            return context.Territories;
        }

        public override Territories GetTerritory(string territoryDescription)
        {
            return context.Territories.Where(x => x.TerritoryDescription.Trim() == territoryDescription).Single();
        }

        public override Regions GetTerritoryRegion(Territories territories)
        {
            return context.Territories.Where(x => x.TerritoryID == territories.TerritoryID).Select(x => x.Region).Single();
        }
    }
}
