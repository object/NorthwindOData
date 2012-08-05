using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Northwind.ServiceTests;
using Northwind.ServiceTests.InMemoryService;

namespace Northwind.ServiceTests.Update
{
    [TestFixture]
    public class InMemoryTests : UpdateTestBase<NorthwindContext,
        Categories, Customers, CustomerDemographics, Employees, Orders, OrderDetails, Products, Regions, Shippers, Suppliers, Territories>
    {
        public InMemoryTests()
            : base(new Northwind.ServiceTests.Query.InMemoryTests())
        {
        }

        public override void ClearTestData()
        {
            this.context.Products.Where(x => x.ProductName.StartsWith("Test_")).ToList()
                .ForEach(x => this.context.DeleteObject(x));
            this.context.Categories.Where(x => x.CategoryName.StartsWith("Test_")).ToList()
                .ForEach(x => this.context.DeleteObject(x));
            this.context.SaveChanges();
        }

        public override string CreateCategory()
        {
            int nextRandom = random.Next();
            var category = new Categories()
            {
                CategoryID = nextRandom,
                CategoryName = "Test_" + nextRandom.ToString()
            };
            this.context.AddToCategories(category);
            this.context.SaveChanges();
            return category.CategoryName;
        }

        public override string UpdateCategory(Categories category)
        {
            int nextRandom = random.Next();
            category.CategoryName = "Test_" + nextRandom.ToString();
            this.context.UpdateObject(category);
            this.context.SaveChanges();
            return category.CategoryName;
        }

        public override void DeleteCategory(Categories category)
        {
            this.context.DeleteObject(category);
            this.context.SaveChanges();
        }

        public override string CreateProduct()
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
            this.context.SaveChanges();
            return product.ProductName;
        }

        public override void AttachProductToCategory(Categories category, Products product)
        {
            this.context.AddLink(category, "Products", product);
            this.context.SaveChanges();
        }

        public override void AssignCategoryToProduct(Products product, Categories category)
        {
            this.context.SetLink(product, "Category", category);
            this.context.SaveChanges();
        }
    }
}
