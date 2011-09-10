using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Services.Common;
using MongoDB.Bson;

namespace Northwind.Mongo.Entities
{
    [DataServiceKey("CategoryID")]
    public class Categories
    {
        public ObjectId _id { get; set; }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public List<Products> Products { get; private set; }

        public Categories()
        {
            this.Products = new List<Products>();
        }
    }
}
