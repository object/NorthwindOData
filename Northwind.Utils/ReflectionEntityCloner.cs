using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Northwind.Utils
{
    public abstract class ReflectionEntityCloner : IProviderEntityCloner
    {
        public abstract object Context { get; }

        public Type GetEntityType(string entityTypeName)
        {
            return this.Context.GetType().GetProperty(entityTypeName).PropertyType.GetGenericArguments()[0].UnderlyingSystemType;
        }

        public object CreateEntityInstance(Type entityType)
        {
            return Activator.CreateInstance(entityType);
        }

        public abstract void AddEntity(object entity, string entityTypeName);

        public abstract void UpdateEntity(object entity);

        public void SetProperty(object entity, string propertyName, object propertyValue)
        {
            var property = entity.GetType().GetProperty(propertyName);
            property.SetValue(entity, ConvertValue(propertyValue, property.PropertyType), null);
        }

        public void AddLink(object entity, string propertyName, object linkedEntity)
        {
            var collectionProperty = entity.GetType().GetProperties()
                .Where(x => x.PropertyType.IsGenericType &&
                            x.PropertyType.GetGenericArguments()[0] == linkedEntity.GetType() &&
                            x.GetValue(entity, null) is IEnumerable<object>).Single();
            var method = collectionProperty.PropertyType.GetMethod("Add");
            method.Invoke(collectionProperty.GetValue(entity, null), new object[] { linkedEntity });
        }

        public void SetLink(object entity, string propertyName, object linkedEntity)
        {
            var property = entity.GetType().GetProperties()
                .Where(x => x.PropertyType == linkedEntity.GetType() &&
                    !(x.GetValue(entity, null) is IEnumerable<object>)).Single();
            property.SetValue(entity, linkedEntity, null);
        }

        public abstract void SaveChanges();

        private object ConvertValue(object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }

            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                var nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }

            return Convert.ChangeType(value, conversionType);
        }
    }
}
