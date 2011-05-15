using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Northwind.ServiceTests
{
    public abstract class UpdateTestBase<
        TContext, TCategories, TCustomers, TCustomerDemographics, TEmployees, TOrders, TOrderDetails, TProducts,
        TRegions, TShippers, TSuppliers, TTerritories>
    {
        protected static Random random = new Random();
        protected TContext context
        {
            get { return queryTestBase.context; }
            set { queryTestBase.context = value; }
        }

        protected QueryTestBase<
            TContext, TCategories, TCustomers, TCustomerDemographics, TEmployees, TOrders, TOrderDetails, TProducts, TRegions, TShippers, TSuppliers, TTerritories> queryTestBase;

        public abstract void ClearTestData();
        public abstract string CreateCategory();
        public abstract string UpdateCategory(TCategories category);
        public abstract void DeleteCategory(TCategories category);
        public abstract string CreateProduct();
        public abstract void AttachProductToCategory(TCategories category, TProducts product);
        public abstract void AssignCategoryToProduct(TProducts product, TCategories category);

        public UpdateTestBase(QueryTestBase<
            TContext, TCategories, TCustomers, TCustomerDemographics, TEmployees, TOrders, TOrderDetails, TProducts, TRegions, TShippers, TSuppliers, TTerritories> queryTestBase)
        {
            this.queryTestBase = queryTestBase;
        }

        [SetUp]
        public void SetUp()
        {
            this.context = queryTestBase.CreateContext();
        }

        [TearDown]
        public void TearDown()
        {
            ClearTestData();
        }

        //[Test]
        //public void should_have_nonnull_context()
        //{
        //    Assert.IsNotNull(context);
        //}

        //[Test]
        //public void should_create_category()
        //{
        //    var categoryName = CreateCategory();
        //    var category = queryTestBase.GetCategory(categoryName);
        //    Assert.IsNotNull(category);
        //}

        //[Test]
        //public void should_update_category()
        //{
        //    var categoryName = CreateCategory();
        //    var category = queryTestBase.GetCategory(categoryName);
        //    var newCategoryName = UpdateCategory(category);
        //    category = queryTestBase.GetCategory(newCategoryName);
        //    Assert.IsNotNull(category);
        //}

        //[Test]
        //public void should_delete_category()
        //{
        //    var categoryName = CreateCategory();
        //    var category = queryTestBase.GetCategory(categoryName);
        //    DeleteCategory(category);
        //    Assert.Throws<InvalidOperationException>(() => queryTestBase.GetCategory(categoryName));
        //}

        [Test]
        public void should_attach_product_to_category()
        {
            var categoryName = CreateCategory();
            var category = queryTestBase.GetCategory(categoryName);
            var productName = CreateProduct();
            var product = queryTestBase.GetProduct(productName);
            AttachProductToCategory(category, product);
            var products = queryTestBase.GetCategoryProducts(category);
            Assert.AreEqual(1, products.Count());
        }

        //[Test]
        //public void should_set_category_to_product()
        //{
        //    var categoryName = CreateCategory();
        //    var category = queryTestBase.GetCategory(categoryName);
        //    var productName = CreateProduct();
        //    var product = queryTestBase.GetProduct(productName);
        //    AssignCategoryToProduct(product, category);
        //    category = queryTestBase.GetProductCategory(product);
        //    Assert.IsNotNull(category);
        //}
    }
}
