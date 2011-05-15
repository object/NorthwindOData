using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm;
using Norm.Collections;
using Norm.Linq;
using Norm.Responses;

namespace Northwind.MongoDB
{
    public partial class MongoContext : IDisposable
    {
        protected string connectionString;
        protected IMongo provider;

        public MongoContext(string connectionString)
        {
            this.connectionString = connectionString;
            this.provider = Mongo.Create(this.connectionString);
        }

        public void Dispose()
        {
            this.provider.Dispose();
        }

        public void SaveChanges()
        {
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return this.provider.Database.GetCollection<T>(typeof(T).Name);
        }

        public IMongoCollection<T> GetCollection<T>(T item)
        {
            return this.provider.Database.GetCollection<T>(item.GetType().Name);
        }

        public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            var items = All<T>().Where(expression);
            foreach (T item in items)
            {
                Delete(item);
            }
        }

        public void Delete<T>(T item) where T : class, new()
        {
            GetCollection<T>().Delete(item);
        }

        public void DeleteAll<T>() where T : class, new()
        {
            this.provider.Database.DropCollection(typeof(T).Name);
        }

        public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression).SingleOrDefault();
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            return GetCollection<T>().AsQueryable();
        }

        public void Add<T>(T item) where T : class, new()
        {
            this.GetCollection<T>(item).Insert(item);
        }

        public void Add<T>(IEnumerable<T> items) where T : class, new()
        {
            foreach (T item in items)
            {
                this.GetCollection<T>(item).Insert(item);
            }
        }

        public void Update<T>(T item) where T : class, new()
        {
            GetCollection<T>(item).UpdateOne(item, item);
        }

        //Helper for using map reduce in mongo
        public T MapReduce<T>(string map, string reduce)
        {
            T result = default(T);
            MapReduce mr = this.provider.Database.CreateMapReduce();

            MapReduceResponse response =
                mr.Execute(new MapReduceOptions(typeof(T).Name)
                {
                    Map = map,
                    Reduce = reduce
                });
            IMongoCollection<MapReduceResult<T>> coll = response.GetCollection<MapReduceResult<T>>();
            MapReduceResult<T> r = coll.Find().FirstOrDefault();
            result = r.Value;

            return result;
        }
    }
}
