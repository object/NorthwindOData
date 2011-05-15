using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Northwind.EntityFramework.ModelFirst.MsSql;
using NUnit.Framework;

namespace Northwind.ModelTests.Update
{
    [TestFixture]
    public class EntityFrameworkModelFirstMsSqlTests : UpdateTestBase<NorthwindMsSqlContext,
        Categories, Customers, CustomerDemographics, Employees, Orders, OrderDetails, Products, Regions, Shippers, Suppliers, Territories>
    {
        public EntityFrameworkModelFirstMsSqlTests()
            : base(new Northwind.ModelTests.Query.EntityFrameworkModelFirstMsSqlTests())
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

        public override void SaveChanges()
        {
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
            return category.CategoryName;
        }

        public override string UpdateCategory(Categories category)
        {
            int nextRandom = random.Next();
            category.CategoryName = "Test_" + nextRandom.ToString();
            return category.CategoryName;
        }

        public override void DeleteCategory(Categories category)
        {
            this.context.DeleteObject(category);
        }

        public override string CreateProduct()
        {
            var category = this.context.Categories.First();
            var supplier = this.context.Suppliers.First();
            int nextRandom = random.Next();
            var product = new Products()
            {
                ProductID = nextRandom,
                ProductName = "Test_" + nextRandom.ToString(),
                CategoryID = category.CategoryID,
                SupplierID = supplier.SupplierID,
            };
            this.context.AddToProducts(product);
            return product.ProductName;
        }

        public override void AttachProductToCategory(Categories category, Products product)
        {
            category.Products.Add(product);
        }

        public override void AssignCategoryToProduct(Products product, Categories category)
        {
            product.Category = category;
        }
    }
}
