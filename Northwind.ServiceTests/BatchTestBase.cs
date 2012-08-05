using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Northwind.ServiceTests
{
    public abstract class BatchTestBase<
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

        public abstract void SaveChanges(SaveChangesOptions options = SaveChangesOptions.Batch);
        public abstract void ClearTestData();
        public abstract TCategories CreateCategory();
        public abstract TCategories CreateInvalidCategory();
        public abstract TCategories UpdateCategory(TCategories category);
        public abstract void DeleteCategory(TCategories category);
        public abstract int GetCategoryID(TCategories category);
        public abstract string GetCategoryName(TCategories category);
        public abstract TProducts CreateProduct();
        public abstract void AttachProductToCategory(TCategories category, TProducts product);
        public abstract void AssignCategoryToProduct(TProducts product, TCategories category);

        public BatchTestBase(QueryTestBase<
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

        [Test]
        public void should_have_nonnull_context()
        {
            Assert.IsNotNull(context);
        }

        [Test]
        public void should_create_category()
        {
            var category = CreateCategory();
            SaveChanges();
            category = queryTestBase.GetCategory(GetCategoryName(category));
            Assert.IsNotNull(category);
        }

        [Test]
        public void should_not_find_unsaved_category()
        {
            var category = CreateCategory();
            Assert.Throws<InvalidOperationException>(() => queryTestBase.GetCategory(GetCategoryName(category)));
        }

        [Test]
        public void should_update_category()
        {
            var category = CreateCategory();
            var oldCategoryName = GetCategoryName(category);
            var newCategory = UpdateCategory(category);
            SaveChanges();
            Assert.AreNotEqual(oldCategoryName, GetCategoryName(newCategory));
            category = queryTestBase.GetCategory(GetCategoryName(newCategory));
            Assert.AreEqual(GetCategoryName(newCategory), GetCategoryName(category));
        }

        [Test]
        public void should_create_and_update_category()
        {
            var category = CreateCategory();
            var savedCategoryName = GetCategoryName(category);
            SaveChanges();
            category = CreateCategory();
            var createdCategoryName = GetCategoryName(category);
            category = queryTestBase.GetCategory(savedCategoryName);
            category = UpdateCategory(category);
            var updatedCategoryName = GetCategoryName(category);
            SaveChanges();
            Assert.AreNotEqual(savedCategoryName, updatedCategoryName);
            Assert.AreNotEqual(createdCategoryName, updatedCategoryName);
            category = queryTestBase.GetCategory(createdCategoryName);
            Assert.IsNotNull(category);
            category = queryTestBase.GetCategory(updatedCategoryName);
            Assert.IsNotNull(category);
            Assert.Throws<InvalidOperationException>(() => queryTestBase.GetCategory(savedCategoryName));
        }

        [Test]
        public void should_delete_category()
        {
            var category = CreateCategory();
            DeleteCategory(category);
            SaveChanges();
            Assert.Throws<InvalidOperationException>(() => queryTestBase.GetCategory(GetCategoryName(category)));
        }

        [Test]
        public void should_not_create_any_category_on_error_in_batch()
        {
            var category = CreateCategory();
            var categoryName = GetCategoryName(category);
            CreateInvalidCategory();
            Assert.Throws<DataServiceRequestException>(() => SaveChanges());
            this.context = queryTestBase.CreateContext();
            Assert.Throws<InvalidOperationException>(() => queryTestBase.GetCategory(categoryName));
        }

        [Test]
        public void should_create_one_category_on_error_in_non_batch_with_stop()
        {
            var category = CreateCategory();
            var categoryName1 = GetCategoryName(category);
            CreateInvalidCategory();
            category = CreateCategory();
            var categoryName2 = GetCategoryName(category);
            Assert.Throws<DataServiceRequestException>(() => SaveChanges(SaveChangesOptions.None));
            this.context = queryTestBase.CreateContext();
            category = queryTestBase.GetCategory(categoryName1);
            Assert.IsNotNull(category);
            Assert.Throws<InvalidOperationException>(() => queryTestBase.GetCategory(categoryName2));
        }

        [Test]
        public void should_create_two_categories_on_error_in_non_batch_with_continue()
        {
            var category = CreateCategory();
            var categoryName1 = GetCategoryName(category);
            CreateInvalidCategory();
            category = CreateCategory();
            var categoryName2 = GetCategoryName(category);
            Assert.Throws<DataServiceRequestException>(() => SaveChanges(SaveChangesOptions.ContinueOnError));
            this.context = queryTestBase.CreateContext();
            category = queryTestBase.GetCategory(categoryName1);
            Assert.IsNotNull(category);
            category = queryTestBase.GetCategory(categoryName2);
            Assert.IsNotNull(category);
        }

        [Test]
        public void should_attach_product_to_category()
        {
            var category = CreateCategory();
            var product = CreateProduct();
            AttachProductToCategory(category, product);
            SaveChanges();
            var products = queryTestBase.GetCategoryProducts(category);
            Assert.AreEqual(1, products.Count());
        }

        [Test]
        public void should_set_category_to_product()
        {
            var category = CreateCategory();
            var product = CreateProduct();
            AssignCategoryToProduct(product, category);
            SaveChanges();
            category = queryTestBase.GetProductCategory(product);
            Assert.IsNotNull(category);
        }
    }
}
