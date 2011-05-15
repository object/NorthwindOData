using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using FluentNHibernate.Mapping;

namespace Northwind.NHibernate.MsSql
{
    [DataServiceKey("CategoryID")]
    public class Categories
    {
        public virtual int CategoryID { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual string Description { get; set; }
        public virtual byte[] Picture { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }

    public class CategoriesMap : ClassMap<Categories>
    {
        public CategoriesMap()
        {
            Id(x => x.CategoryID);
            Map(x => x.CategoryName).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.Picture);
            HasMany(x => x.Products)
                .Table("Products")
                .KeyColumn("CategoryID");
        }
    }
}
