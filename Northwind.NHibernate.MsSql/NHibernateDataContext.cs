using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.Reflection;
using System.Web;
using NHibernate;

namespace Northwind.NHibernate.MsSql
{
    public abstract class NHibernateDataContext : IUpdatable
    {
        protected ISession Session;

        private List<object> entityToUpdate = new List<object>();
        private List<object> entityToRemove = new List<object>();

        public NHibernateDataContext(ISession session)
        {
            this.Session = session;
        }

        object IUpdatable.CreateResource(string containerName, string fullTypeName)
        {
            var type = Type.GetType(fullTypeName, true);
            var resource = Activator.CreateInstance(type);

            entityToUpdate.Add(resource);

            return resource;
        }

        object IUpdatable.GetResource(IQueryable query, string fullTypeName)
        {
            object resource = null;

            foreach (object item in query)
            {
                if (resource != null)
                {
                    throw new DataServiceException("The query must return a single resource");
                }
                resource = item;
            }

            if (resource == null)
                throw new DataServiceException(404, "Resource not found");

            // fullTypeName can be null for deletes
            if (fullTypeName != null && resource.GetType().FullName != fullTypeName)
                throw new Exception("Unexpected type for resource");

            return resource;
        }

        object IUpdatable.ResetResource(object resource)
        {
            return resource;
        }

        void IUpdatable.SetValue(object targetResource, string propertyName, object propertyValue)
        {
            var propertyInfo = targetResource.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(targetResource, propertyValue, null);

            if (!entityToUpdate.Contains(targetResource))
                entityToUpdate.Add(targetResource);
        }

        object IUpdatable.GetValue(object targetResource, string propertyName)
        {
            var propertyInfo = targetResource.GetType().GetProperty(propertyName);
            return propertyInfo.GetValue(targetResource, null);
        }

        void IUpdatable.SetReference(object targetResource, string propertyName, object propertyValue)
        {
            ((IUpdatable)this).SetValue(targetResource, propertyName, propertyValue);
        }

        void IUpdatable.AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded)
        {
            var propertyInfo = targetResource.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
                throw new Exception("Can't find property");

            var collection = (IList)propertyInfo.GetValue(targetResource, null);
            collection.Add(resourceToBeAdded);

            if (!entityToUpdate.Contains(targetResource))
                entityToUpdate.Add(targetResource);
        }

        void IUpdatable.RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved)
        {
            var propertyInfo = targetResource.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
                throw new Exception("Can't find property");
            var collection = (IList)propertyInfo.GetValue(targetResource, null);
            collection.Remove(resourceToBeRemoved);

            if (!entityToUpdate.Contains(targetResource))
                entityToUpdate.Add(targetResource);
        }

        void IUpdatable.DeleteResource(object targetResource)
        {
            entityToRemove.Add(targetResource);
        }

        void IUpdatable.SaveChanges()
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.FlushMode = FlushMode.Commit;

                foreach (var entity in entityToUpdate)
                {
                    Session.SaveOrUpdate(entity);
                }

                foreach (var entity in entityToRemove)
                {
                    Session.Delete(entity);
                }

                transaction.Commit();
            }

        }

        object IUpdatable.ResolveResource(object resource)
        {
            return resource;
        }

        void IUpdatable.ClearChanges()
        {
        }
    }
}
