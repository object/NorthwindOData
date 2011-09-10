using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Utils
{
    public interface IProviderEntityCloner
    {
        Type GetEntityType(string entityTypeName);
        object CreateEntityInstance(Type entityType);
        void AddEntity(object entity, string entityTypeName);
        void UpdateEntity(object entity);
        void SetProperty(object entity, string propertyName, object propertyValue);
        void AddLink(object entity, string propertyName, object linkedEntity);
        void SetLink(object entity, string propertyName, object linkedEntity);
        void SaveChanges();
    }
}
