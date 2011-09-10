using System;
using System.Data.Services.Common;
using System.Linq;
using MongoDB.Bson;

namespace Northwind.Mongo.Entities
{
    [DataServiceKey(new string[] { "OrderID", "ProductID" })]
    public class OrderDetails
    {
        public ObjectId _id { get; set; }

        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }

        public Orders Order { get; set; }
        public Products Product { get; set; }
    }
}
