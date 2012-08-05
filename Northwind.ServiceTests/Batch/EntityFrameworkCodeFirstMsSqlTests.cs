using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.ServiceTests;
using Northwind.ServiceTests.EntityFrameworkCodeFirstMsSqlService;

namespace Northwind.ServiceTests.Batch
{
    [TestFixture]
    public class EntityFrameworkCodeFirstMsSqlTests : BatchTestBase<NorthwindContext,
        Categories, Customers, CustomerDemographics, Employees, Orders, OrderDetails, Products, Regions, Shippers, Suppliers, Territories>
    {
        public EntityFrameworkCodeFirstMsSqlTests()
            : base(new Northwind.ServiceTests.Query.EntityFrameworkCodeFirstMsSqlTests())
        {
        }

        public override void SaveChanges(SaveChangesOptions options = SaveChangesOptions.Batch)
        {
            this.context.SaveChanges(options);
        }

        public override void ClearTestData()
        {
            this.context.Products.Where(x => x.ProductName.StartsWith("Test_")).ToList()
                .ForEach(x => this.context.DeleteObject(x));
            this.context.Categories.Where(x => x.CategoryName.StartsWith("Test_")).ToList()
                .ForEach(x => this.context.DeleteObject(x));
            this.context.SaveChanges();
        }

        public override Categories CreateCategory()
        {
            int nextRandom = random.Next();
            var category = new Categories()
            {
                CategoryID = nextRandom,
                CategoryName = "Test_" + nextRandom.ToString()
            };
            this.context.AddToCategories(category);
            return category;
        }

        public override Categories CreateInvalidCategory()
        {
            var category = new Categories();
            this.context.AddToCategories(category);
            return category;
        }

        public override Categories UpdateCategory(Categories category)
        {
            int nextRandom = random.Next();
            category.CategoryName = "Test_" + nextRandom.ToString();
            this.context.UpdateObject(category);
            return category;
        }

        public override void DeleteCategory(Categories category)
        {
            this.context.DeleteObject(category);
        }

        public override int GetCategoryID(Categories category)
        {
            return category.CategoryID;
        }

        public override string GetCategoryName(Categories category)
        {
            return category.CategoryName;
        }

        public override Products CreateProduct()
        {
            int nextRandom = random.Next();
            var product = new Products()
            {
                ProductID = nextRandom,
                ProductName = "Test_" + nextRandom.ToString(),
                CategoryID = 1,
                SupplierID = 1,
            };
            this.context.AddToProducts(product);
            return product;
        }

        public override void AttachProductToCategory(Categories category, Products product)
        {
            this.context.AddLink(category, "Products", product);
        }

        public override void AssignCategoryToProduct(Products product, Categories category)
        {
            this.context.SetLink(product, "Category", category);
        }
    }
}
