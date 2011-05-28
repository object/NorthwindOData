using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;
using Northwind.Utils;

namespace Northwind.Mongo.Entities
{
    public class NorthwindContext : MongoContext
    {
        public NorthwindContext(string connectionString)
            : base(connectionString)
        {
        }

        public MongoCollection<Categories> Categories { get { return this.database.GetCollection<Categories>("Categories"); } }
        public MongoCollection<Customers> Customers { get { return this.database.GetCollection<Customers>("Customers"); } }
        public MongoCollection<CustomerDemographics> CustomerDemographics { get { return this.database.GetCollection<CustomerDemographics>("CustomerDemographics"); } }
        public MongoCollection<Employees> Employees { get { return this.database.GetCollection<Employees>("Employees"); } }
        public MongoCollection<Orders> Orders { get { return this.database.GetCollection<Orders>("Orders"); } }
        public MongoCollection<OrderDetails> OrderDetails { get { return this.database.GetCollection<OrderDetails>("OrderDetails"); } }
        public MongoCollection<Products> Products { get { return this.database.GetCollection<Products>("Products"); } }
        public MongoCollection<Regions> Regions { get { return this.database.GetCollection<Regions>("Regions"); } }
        public MongoCollection<Shippers> Shippers { get { return this.database.GetCollection<Shippers>("Shippers"); } }
        public MongoCollection<Suppliers> Suppliers { get { return this.database.GetCollection<Suppliers>("Suppliers"); } }
        public MongoCollection<Territories> Territories { get { return this.database.GetCollection<Territories>("Territories"); } }

        public void Populate(object sourceContext)
        {
            var entityCloner = new EntityCloner(sourceContext, new MongoEntityCloner(this));
            entityCloner.CopyEntities();
        }

        class MongoEntityCloner : IProviderEntityCloner, IDisposable
        {
            private NorthwindContext context;
            private Dictionary<ObjectId, string> entityCollectionMap = new Dictionary<ObjectId, string>();

            public MongoEntityCloner(NorthwindContext context)
            {
                this.context = context;
                context.database.Drop();
            }

            public void Dispose()
            {
                this.context.Dispose();
            }

            public Type GetEntityType(string entityTypeName)
            {
                return typeof(BsonDocument);
            }

            public object CreateEntityInstance(Type entityType)
            {
                return new BsonDocument();
            }

            public void AddEntity(object entity, string entityTypeName)
            {
                var collection = this.context.Database.GetCollection<BsonDocument>(entityTypeName);
                collection.Insert(entity);
                this.entityCollectionMap.Add((entity as BsonDocument)["_id"].AsObjectId, entityTypeName);
            }

            public void UpdateEntity(object entity)
            {
            }

            public BsonDocument CopyEntity(object entity)
            {
                var sourceEntity = entity as BsonDocument;
                var bsonDocument = new BsonDocument();
                IDictionary<string, object> properties = new Dictionary<string, object>();
                foreach (var element in sourceEntity.Elements)
                {
                    SetProperty(bsonDocument, element.Name, element.Value);
                }
                bsonDocument.Add(properties);
                return bsonDocument;
            }

            public void SetProperty(object entity, string propertyName, object propertyValue)
            {
                (entity as BsonDocument).Add(propertyName, BsonValue.Create(ConvertValue(propertyValue)));
            }

            public void AddLink(object entity, string propertyName, object linkedEntity)
            {
                var bsonEntity = entity as BsonDocument;
                if (!bsonEntity.Contains(propertyName))
                {
                    bsonEntity.Add(propertyName, new BsonArray());
                }
                var linkedCollection = bsonEntity[propertyName].AsBsonArray;
                linkedCollection.Add(BsonValue.Create(CopyEntity(linkedEntity)));
                LinkValue(entity, propertyName, linkedCollection);
            }

            public void SetLink(object entity, string propertyName, object linkedEntity)
            {
                LinkValue(entity, propertyName, CopyEntity(linkedEntity));
            }

            public void SaveChanges()
            {
            }

            private void LinkValue(object entity, string propertyName, BsonValue linkedValue)
            {
                var bsonEntity = entity as BsonDocument;
                var objectId = bsonEntity["_id"].AsObjectId;
                var collectionName = this.entityCollectionMap[objectId];
                var collection = this.context.Database.GetCollection<BsonDocument>(collectionName);
                collection.Update(Query.EQ("_id", objectId), Update.Set(propertyName, linkedValue));
            }

            private object ConvertValue(object value)
            {
                if (value == null)
                    return value;

                if (value.GetType() == typeof(decimal))
                {
                    return Convert.ToDouble(value);
                }
                else
                {
                    return value;
                }
            }
        }
    }
}
