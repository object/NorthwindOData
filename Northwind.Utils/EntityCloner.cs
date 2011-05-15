using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Northwind.Utils
{
    public class EntityCloner
    {
        enum PropertyRelationType
        {
            Column,
            Reference,
            ReferencedByOne,
            ReferencedByMany,
            System,
            Unknown
        }

        class EntityRelation
        {
            public PropertyInfo EntityProperty { get; set; }
            public Type PropertyEntityType { get; set; }
            public PropertyRelationType PropertyRelationType { get; set; }

            public override bool Equals(object obj)
            {
                if (obj is EntityRelation)
                {
                    var er = obj as EntityRelation;
                    return this.EntityProperty == er.EntityProperty &&
                        this.PropertyEntityType == er.PropertyEntityType &&
                        this.PropertyRelationType == er.PropertyRelationType;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return this.EntityProperty.GetHashCode() ^ this.PropertyEntityType.GetHashCode() ^ this.PropertyRelationType.GetHashCode();
            }
        }

        private object sourceContext;
        private object destinationContext;
        private Action<object> AddObjectToContext;
        private Func<Type, IEnumerable<object>> GetContextEntityContainer;
        private Action SaveChanges;
        private readonly List<string> systemProperties = new List<string> { "EntityState", "EntityKey" };
        private Dictionary<EntityKey, object> entityMap = new Dictionary<EntityKey, object>();
        private Dictionary<string, List<EntityRelation>> entityPropertyMap = new Dictionary<string, List<EntityRelation>>();

        public EntityCloner(object sourceContext, object destinationContext,
            Action<object> addObjectToContext, 
            Func<Type, IEnumerable<object>> getContextEntityContainer,
            Action saveChanges)
        {
            this.sourceContext = sourceContext;
            this.destinationContext = destinationContext;
            this.AddObjectToContext = addObjectToContext;
            this.GetContextEntityContainer = getContextEntityContainer;
            this.SaveChanges = saveChanges;
        }

        public void CopyEntities()
        {
            Action<PropertyInfo, PropertyInfo, object>[] actions =
                new Action<PropertyInfo, PropertyInfo, object>[]
                    {
                        (x, y, z) => CopyEntityProperties(z, y.PropertyType),
                        (x, y, z) => CopyEntityRelations(z, this.entityPropertyMap[x.PropertyType.GetGenericArguments()[0].Name])
                    };

            foreach (var action in actions)
            {
                foreach (var sourceContainerProperty in sourceContext.GetType().GetProperties())
                {
                    var sourceContainer = sourceContainerProperty.GetValue(sourceContext, null) as IEnumerable<object>;
                    if (sourceContainer != null)
                    {
                        var destinationContainerProperty = destinationContext.GetType().GetProperty(sourceContainerProperty.Name);

                        if (destinationContainerProperty != null)
                        {
                            foreach (var item in sourceContainer)
                            {
                                action(sourceContainerProperty, destinationContainerProperty, item);
                            }
                        }
                    }
                }
                SaveChanges();
            }
        }

        private void CopyEntityProperties(object sourceObject, Type entityContainerType)
        {
            var entityType = entityContainerType.GetGenericArguments()[0].UnderlyingSystemType;
            if (!this.entityPropertyMap.ContainsKey(entityType.Name))
                this.entityPropertyMap.Add(entityType.Name, new List<EntityRelation>());
            var entityProperties = this.entityPropertyMap[entityType.Name];

            var entity = Activator.CreateInstance(entityType);

            foreach (var propertyInfo in sourceObject.GetType().GetProperties())
            {
                var entityProperty = entityProperties.Where(x => x.EntityProperty == propertyInfo).SingleOrDefault();
                if (entityProperty == null)
                {
                    var propertyRelationType = GetPropertyRelationType(propertyInfo);
                    entityProperty = new EntityRelation()
                    {
                        EntityProperty = propertyInfo,
                        PropertyEntityType = propertyInfo.PropertyType.IsGenericType ?
                                            propertyInfo.PropertyType.GetGenericArguments()[0] :
                                            propertyInfo.PropertyType,
                        PropertyRelationType = propertyRelationType
                    };
                }
                if (!entityProperties.Contains(entityProperty))
                    entityProperties.Add(entityProperty);

                if (entityProperty.PropertyRelationType == PropertyRelationType.Column)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject, null);
                    foreach (var prop in entity.GetType().GetProperties())
                    {
                        if (string.Compare(prop.Name, propertyInfo.Name, true) == 0 && propertyValue != null)
                        {
                            prop.SetValue(entity, ConvertValue(propertyValue, prop.PropertyType), null);
                        }
                    }
                }
            }

            var entityKey = sourceObject is EntityReference ? (sourceObject as EntityReference).EntityKey : (sourceObject as EntityObject).EntityKey;
            this.entityMap.Add(entityKey, entity);

            AddObjectToContext(entity);
        }

        private void CopyEntityRelations(object sourceObject, List<EntityRelation> entityRelations)
        {
            foreach (var entityRelation in entityRelations.Where(x =>
                x.PropertyRelationType == PropertyRelationType.Reference ||
                x.PropertyRelationType == PropertyRelationType.ReferencedByMany))
            {
                var propertyValue = entityRelation.EntityProperty.GetValue(sourceObject, null);
                if (propertyValue != null)
                {
                    switch (entityRelation.PropertyRelationType)
                    {
                        case PropertyRelationType.Reference:
                            CopyManyToOneRelation(sourceObject, propertyValue);
                            break;

                        case PropertyRelationType.ReferencedByMany:
                            CopyOneToManyRelation(sourceObject, propertyValue);
                            CopyManyToManyRelation(sourceObject, propertyValue);
                            break;
                    }
                }
            }
        }

        private void CopyManyToOneRelation(object sourceObject, object propertyValue)
        {
            var referencedEntity = propertyValue;
            foreach (var entityProperty in GetRelationProperties(sourceObject, referencedEntity.GetType(), PropertyRelationType.ReferencedByMany))
            {
                var referencedContainer = entityProperty.EntityProperty.GetValue(referencedEntity, null) as IEnumerable<object>;
                if (referencedContainer != null)
                {
                    foreach (var referringEntity in referencedContainer)
                    {
                        AddRelationToEntity(referringEntity, referencedEntity);
                    }
                }
            }
        }

        private void CopyOneToManyRelation(object sourceObject, object propertyValue)
        {
            foreach (var referencedEntity in propertyValue as IEnumerable<object>)
            {
                foreach (var entityProperty in GetRelationProperties(sourceObject, referencedEntity.GetType(), PropertyRelationType.Reference))
                {
                    var referringEntity = entityProperty.EntityProperty.GetValue(referencedEntity, null);
                    if (referringEntity != null)
                    {
                        AddRelationToEntity(referringEntity, referencedEntity);
                    }
                }
            }
        }

        private void CopyManyToManyRelation(object sourceObject, object propertyValue)
        {
            foreach (var referencedEntity in propertyValue as IEnumerable<object>)
            {
                foreach (var entityProperty in GetRelationProperties(sourceObject, referencedEntity.GetType(), PropertyRelationType.ReferencedByMany))
                {
                    var referencedContainer = entityProperty.EntityProperty.GetValue(referencedEntity, null) as IEnumerable<object>;
                    if (referencedContainer != null)
                    {
                        foreach (var referringEntity in referencedContainer)
                        {
                            AddRelationToEntity(referringEntity, referencedEntity);
                        }
                    }
                }
            }
        }

        private IEnumerable<EntityRelation> GetRelationProperties(object sourceObject, Type referencedEntityType, PropertyRelationType relationType)
        {
            return this.entityPropertyMap[referencedEntityType.Name]
                .Where(x => x.PropertyEntityType == sourceObject.GetType() &&
                            (x.PropertyRelationType == relationType));
        }

        private void AddRelationToEntity(object referringEntity, object referencedEntity)
        {
            foreach (var entityProperty in this.entityPropertyMap[referringEntity.GetType().Name]
                .Where(x => x.PropertyEntityType == referencedEntity.GetType() &&
                    (x.PropertyRelationType == PropertyRelationType.Reference || x.PropertyRelationType == PropertyRelationType.ReferencedByMany)))
            {
                Action<object, object> linkAction;
                switch (entityProperty.PropertyRelationType)
                {
                    case PropertyRelationType.Reference:
                        LinkEntities(referringEntity, referencedEntity, SetLink);
                        break;
                    case PropertyRelationType.ReferencedByMany:
                        LinkEntities(referringEntity, referencedEntity, AddLink);
                        break;
                }
            }
        }

        private void LinkEntities(object referringEntity, object referencedEntity, Action<object, object> linkAction)
        {
            var destinationEntity = FindEntity(referringEntity);
            var destinationReferencedEntity = FindEntity(referencedEntity);
            if (destinationEntity == null || destinationReferencedEntity == null) return;
            linkAction(destinationEntity, destinationReferencedEntity);
        }

        private void AddLink(object referringEntity, object referencedEntity)
        {
            var collectionProperty = referringEntity.GetType().GetProperties()
                .Where(x => x.PropertyType.IsGenericType &&
                            x.PropertyType.GetGenericArguments()[0] == referencedEntity.GetType() &&
                            x.GetValue(referringEntity, null) is IEnumerable<object>).Single();
            var method = collectionProperty.PropertyType.GetMethod("Add");
            method.Invoke(collectionProperty.GetValue(referringEntity, null), new object[] { referencedEntity });
        }

        private void SetLink(object referringEntity, object referencedEntity)
        {
            var property = referringEntity.GetType().GetProperties()
                .Where(x => x.PropertyType == referencedEntity.GetType() && 
                    !(x.GetValue(referringEntity, null) is IEnumerable<object>)).Single();
            property.SetValue(referringEntity, referencedEntity, null);
        }

        private object FindEntity(object entity)
        {
            var entityKey = entity is EntityReference ? (entity as EntityReference).EntityKey : (entity as EntityObject).EntityKey;
            return this.entityMap[entityKey];
        }

        private PropertyRelationType GetPropertyRelationType(PropertyInfo propertyInfo)
        {
            if (this.systemProperties.Contains(propertyInfo.Name))
                return PropertyRelationType.System;
            else if (!propertyInfo.PropertyType.IsClass ||
                propertyInfo.PropertyType == typeof(string) ||
                propertyInfo.PropertyType == typeof(byte[]))
                return PropertyRelationType.Column;
            else if (propertyInfo.PropertyType.Name == "EntityReference`1")
                return PropertyRelationType.ReferencedByOne;
            else if (propertyInfo.PropertyType.Name == "EntityCollection`1")
                return PropertyRelationType.ReferencedByMany;
            else if (propertyInfo.PropertyType.GetInterface("IEntityWithKey") != null)
                return PropertyRelationType.Reference;
            else
                return PropertyRelationType.Unknown;
        }

        private bool EntityKeysEqual(object sourceEntity, object destinationEntity)
        {
            if (sourceEntity.GetType().Name != destinationEntity.GetType().Name)
                return false;

            var entityKey = sourceEntity is EntityReference ? (sourceEntity as EntityReference).EntityKey : (sourceEntity as EntityObject).EntityKey;
            foreach (var keyPropertyInfo in entityKey.EntityKeyValues)
            {
                var destinationPropertyInfo = destinationEntity.GetType().GetProperty(keyPropertyInfo.Key);
                if (destinationPropertyInfo != null)
                {
                    var sourceValue = keyPropertyInfo.Value;
                    var destinationValue = destinationPropertyInfo.GetValue(destinationEntity, null);
                    if (sourceValue == null && destinationValue != null || !sourceValue.Equals(destinationValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

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
