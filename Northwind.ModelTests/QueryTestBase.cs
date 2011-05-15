using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Northwind.ModelTests
{
    public abstract class QueryTestBase<
        TContext, TCategories, TCustomers, TCustomerDemographics, TEmployees, TOrders, TOrderDetails, TProducts, TRegions, TShippers, TSuppliers, TTerritories>
    {
        internal TContext context;

        public abstract TContext CreateContext();
        public abstract void DisposeContext(TContext context);
        public abstract IQueryable<TCategories> GetCategories();
        public abstract TCategories GetCategory(string categoryName);
        public abstract IQueryable<TProducts> GetCategoryProducts(TCategories category);
        public abstract IQueryable<TCustomers> GetCustomers();
        public abstract TCustomers GetCustomer(string customerID);
        public abstract IQueryable<TOrders> GetCustomerOrders(TCustomers customer);
        public abstract IQueryable<TCustomerDemographics> GetCustomerCustomerDemographics(TCustomers customer);
        public abstract IQueryable<TCustomerDemographics> GetCustomerDemographics();
        public abstract TCustomerDemographics GetCustomerDemographics(string customerTypeID);
        public abstract IQueryable<TCustomers> GetCustomerDemographicsCustomers(TCustomerDemographics customerDemographics);
        public abstract IQueryable<TEmployees> GetEmployees();
        public abstract TEmployees GetEmployee(string firstName, string lastName);
        public abstract IQueryable<TEmployees> GetEmployeeSubordinates(TEmployees employee);
        public abstract TEmployees GetEmployeeSuperior(TEmployees employee);
        public abstract IQueryable<TOrders> GetEmployeeOrders(TEmployees employee);
        public abstract IQueryable<TTerritories> GetEmployeeTerritories(TEmployees employee);
        public abstract IQueryable<TOrders> GetOrders();
        public abstract TOrders GetOrder(int orderID);
        public abstract IQueryable<TOrderDetails> GetOrderOrderDetails(TOrders order);
        public abstract TCustomers GetOrderCustomer(TOrders order);
        public abstract TEmployees GetOrderEmployee(TOrders order);
        public abstract TShippers GetOrderShipper(TOrders order);
        public abstract IQueryable<TOrderDetails> GetOrderDetails();
        public abstract TOrderDetails GetOrderDetails(int orderID, int productID);
        public abstract TOrders GetOrderDetailsOrder(TOrderDetails orderDetails);
        public abstract TProducts GetOrderDetailsProduct(TOrderDetails orderDetails);
        public abstract IQueryable<TProducts> GetProducts();
        public abstract TProducts GetProduct(string productName);
        public abstract TCategories GetProductCategory(TProducts product);
        public abstract TSuppliers GetProductSupplier(TProducts product);
        public abstract IQueryable<TOrderDetails> GetProductOrderDetails(TProducts product);
        public abstract IQueryable<TRegions> GetRegions();
        public abstract TRegions GetRegion(string regionDescription);
        public abstract IQueryable<TTerritories> GetRegionTerritories(TRegions region);
        public abstract IQueryable<TShippers> GetShippers();
        public abstract TShippers GetShipper(string companyName);
        public abstract IQueryable<TOrders> GetShipperOrders(TShippers shipper);
        public abstract IQueryable<TSuppliers> GetSuppliers();
        public abstract TSuppliers GetSupplier(string companyName);
        public abstract IQueryable<TProducts> GetSupplierProducts(TSuppliers suppliers);
        public abstract IQueryable<TTerritories> GetTerritories();
        public abstract TTerritories GetTerritory(string territoryDescription);
        public abstract TRegions GetTerritoryRegion(TTerritories territories);

        [SetUp]
        public void SetUp()
        {
            this.context = CreateContext();
        }

        [TearDown]
        public void TearDown()
        {
            DisposeContext(this.context);
        }

        [Test]
        public void should_have_nonnull_context()
        {
            Assert.IsNotNull(context);
        }

        [Test]
        public void should_retrieve_nonempty_categories()
        {
            var categories = GetCategories();
            Assert.Greater(categories.Count(), 0);
            var c = categories.First();
        }

        [Test]
        public void should_retrieve_nonempty_category_products()
        {
            var category = GetCategory("Beverages");
            var products = GetCategoryProducts(category);
            Assert.Greater(products.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_customers()
        {
            var customers = GetCustomers();
            Assert.Greater(customers.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_customer_orders()
        {
            var customer = GetCustomer("ALFKI");
            var customerOrders = GetCustomerOrders(customer);
            Assert.Greater(customerOrders.Count(), 0);
        }

        [Test]
        public void should_retrieve_empty_customer_customer_demographics()
        {
            var customer = GetCustomer("ALFKI");
            var customerDemographics = GetCustomerCustomerDemographics(customer);
            Assert.AreEqual(0, customerDemographics.Count());
        }

        [Test]
        public void should_retrieve_empty_customer_demographics()
        {
            var customerDemographics = GetCustomerDemographics();
            Assert.AreEqual(0, customerDemographics.Count());
        }

        [Test]
        public void should_retrieve_empty_customer_demographics_customer()
        {
            try
            {
                var customerDemographics = GetCustomerDemographics(string.Empty);
                var customers = GetCustomerDemographicsCustomers(customerDemographics);
                Assert.AreEqual(0, customers.Count());
            }
            catch (InvalidOperationException)
            {
                // No data in CustomerDemographics table, swallow the exception
            }
        }

        [Test]
        public void should_retrieve_nonempty_employes()
        {
            var employees = GetEmployees();
            Assert.Greater(employees.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_employee_subordinates()
        {
            var employee = GetEmployee("Andrew", "Fuller");
            var subordinates = GetEmployeeSubordinates(employee);
            Assert.Greater(subordinates.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonnull_employee_superior()
        {
            var employee = GetEmployee("Nancy", "Davolio");
            var superior = GetEmployeeSuperior(employee);
            Assert.IsNotNull(superior);
        }

        [Test]
        public void should_retrieve_nonempty_employee_orders()
        {
            var employee = GetEmployee("Andrew", "Fuller");
            var orders = GetEmployeeOrders(employee);
            Assert.Greater(orders.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_employee_territories()
        {
            var employee = GetEmployee("Andrew", "Fuller");
            var territories = GetEmployeeTerritories(employee);
            Assert.Greater(territories.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_orders()
        {
            var orders = GetOrders();
            Assert.Greater(orders.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_order_order_details()
        {
            var order = GetOrder(10248);
            var orderDetails = GetOrderOrderDetails(order);
            Assert.Greater(orderDetails.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonnull_order_customer()
        {
            var order = GetOrder(10248);
            var customer = GetOrderCustomer(order);
            Assert.IsNotNull(customer);
        }

        [Test]
        public void should_retrieve_nonnull_order_employee()
        {
            var order = GetOrder(10248);
            var employee = GetOrderEmployee(order);
            Assert.IsNotNull(employee);
        }

        [Test]
        public void should_retrieve_nonnull_order_shipper()
        {
            var order = GetOrder(10248);
            var shipper = GetOrderShipper(order);
            Assert.IsNotNull(shipper);
        }

        [Test]
        public void should_retrieve_nonempty_order_details()
        {
            var orderDetails = GetOrderDetails();
            Assert.Greater(orderDetails.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonnull_order_details_order()
        {
            var orderDetails = GetOrderDetails(10248, 11);
            var order = GetOrderDetailsOrder(orderDetails);
            Assert.IsNotNull(order);
        }

        [Test]
        public void should_retrieve_nonnull_order_details_product()
        {
            var orderDetails = GetOrderDetails(10248, 11);
            var product = GetOrderDetailsProduct(orderDetails);
            Assert.IsNotNull(product);
        }

        [Test]
        public void should_retrieve_nonempty_products()
        {
            var products = GetProducts();
            Assert.Greater(products.Count(), 0);
            var p = products.First();
        }

        [Test]
        public void should_retrieve_nonnull_product_category()
        {
            var product = GetProduct("Chai");
            var category = GetProductCategory(product);
            Assert.IsNotNull(category);
        }

        [Test]
        public void should_retrieve_nonnull_product_supplier()
        {
            var product = GetProduct("Chai");
            var supplier = GetProductSupplier(product);
            Assert.IsNotNull(supplier);
        }

        [Test]
        public void should_retrieve_nonempty_product_order_details()
        {
            var product = GetProduct("Chai");
            var orderDetails = GetProductOrderDetails(product);
            Assert.Greater(orderDetails.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_regions()
        {
            var regions = GetRegions();
            Assert.Greater(regions.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_region_territories()
        {
            var region = GetRegion("Eastern");
            var territories = GetRegionTerritories(region);
            Assert.Greater(territories.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_shippers()
        {
            var shippers = GetShippers();
            Assert.Greater(shippers.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_shipper_orders()
        {
            var shipper = GetShipper("Speedy Express");
            var orders = GetShipperOrders(shipper);
            Assert.Greater(orders.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_suppliers()
        {
            var suppliers = GetSuppliers();
            Assert.Greater(suppliers.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_supplier_products()
        {
            var supplier = GetSupplier("Exotic Liquids");
            var products = GetSupplierProducts(supplier);
            Assert.Greater(products.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonempty_territories()
        {
            var territories = GetTerritories();
            Assert.Greater(territories.Count(), 0);
        }

        [Test]
        public void should_retrieve_nonnull_territory_region()
        {
            var territory = GetTerritory("Westboro");
            var region = GetTerritoryRegion(territory);
            Assert.IsNotNull(region);
        }
    }
}
