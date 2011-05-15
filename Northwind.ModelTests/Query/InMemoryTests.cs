using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.InMemory;

namespace Northwind.ModelTests.Query
{
    [TestFixture]
    public class InMemoryTests : QueryTestBase<NorthwindContext,
        Categories, Customers, CustomerDemographics, Employees, Orders, OrderDetails, Products, Regions, Shippers, Suppliers, Territories>
    {
        public override NorthwindContext CreateContext()
        {
            return NorthwindContext.Instance;
        }

        public override IQueryable<Categories> GetCategories()
        {
            return this.context.Categories;
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
            return this.context.Customers;
        }

        public override Customers GetCustomer(string customerID)
        {
            return this.context.Customers.Where(x => x.CustomerID == customerID).Single();
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
            return this.context.Employees;
        }

        public override Employees GetEmployee(string firstName, string lastName)
        {
            return this.context.Employees.Where(x => x.FirstName == firstName && x.LastName == lastName).Single();
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
            return this.context.Orders;
        }

        public override Orders GetOrder(int orderID)
        {
            return this.context.Orders.Where(x => x.OrderID == orderID).Single();
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
            return this.context.OrderDetails;
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
            return this.context.Products;
        }

        public override Products GetProduct(string productName)
        {
            return this.context.Products.Where(x => x.ProductName == productName).Single();
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
            return this.context.Regions;
        }

        public override Regions GetRegion(string regionDescription)
        {
            return this.context.Regions.Where(x => x.RegionDescription.Trim() == regionDescription).Single();
        }

        public override IQueryable<Territories> GetRegionTerritories(Regions region)
        {
            return region.Territories.AsQueryable();
        }

        public override IQueryable<Shippers> GetShippers()
        {
            return this.context.Shippers;
        }

        public override Shippers GetShipper(string companyName)
        {
            return this.context.Shippers.Where(x => x.CompanyName == companyName).Single();
        }

        public override IQueryable<Orders> GetShipperOrders(Shippers shipper)
        {
            return shipper.Orders.AsQueryable();
        }

        public override IQueryable<Suppliers> GetSuppliers()
        {
            return this.context.Suppliers;
        }

        public override Suppliers GetSupplier(string companyName)
        {
            return this.context.Suppliers.Where(x => x.CompanyName == companyName).Single();
        }

        public override IQueryable<Products> GetSupplierProducts(Suppliers suppliers)
        {
            return suppliers.Products.AsQueryable();
        }

        public override IQueryable<Territories> GetTerritories()
        {
            return this.context.Territories;
        }

        public override Territories GetTerritory(string territoryDescription)
        {
            return this.context.Territories.Where(x => x.TerritoryDescription.Trim() == territoryDescription).Single();
        }

        public override Regions GetTerritoryRegion(Territories territories)
        {
            return territories.Region;
        }
    }
}
