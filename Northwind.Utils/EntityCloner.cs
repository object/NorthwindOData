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
        private IProviderEntityCloner destinationContext;
        private readonly List<string> systemProperties = new List<string> { "EntityState", "EntityKey" };
        private Dictionary<string, Type> entityTypeMap = new Dictionary<string, Type>();
        private Dictionary<EntityKey, object> entityKeyMap = new Dictionary<EntityKey, object>();
        private Dictionary<string, List<EntityRelation>> entityPropertyMap = new Dictionary<string, List<EntityRelation>>();

        public EntityCloner(object sourceContext, IProviderEntityCloner destinationContext)
        {
            this.sourceContext = sourceContext;
            this.destinationContext = destinationContext;
        }

        public void CopyEntities()
        {
            Action<object, string>[] actions =
                new Action<object, string>[]
                    {
                        (x, y) => CopyEntityProperties(x, y),
                        (x, y) => CopyEntityRelations(x, y, this.entityPropertyMap[y])
                    };

            foreach (var action in actions)
            {
                foreach (var sourceContainerProperty in sourceContext.GetType().GetProperties())
                {
                    var sourceContainer = sourceContainerProperty.GetValue(sourceContext, null) as IEnumerable<object>;
                    if (sourceContainer != null)
                    {
                        foreach (var item in sourceContainer)
                        {
                            action(item, sourceContainerProperty.Name);
                        }
                    }
                }
                this.destinationContext.SaveChanges();
            }
        }

        private void CopyEntityProperties(object sourceObject, string entityTypeName)
        {
            Type entityType;
            if (!this.entityTypeMap.ContainsKey(entityTypeName))
                this.entityTypeMap.Add(entityTypeName, this.destinationContext.GetEntityType(entityTypeName));
            entityType = this.entityTypeMap[entityTypeName];

            if (!this.entityPropertyMap.ContainsKey(entityTypeName))
                this.entityPropertyMap.Add(entityTypeName, new List<EntityRelation>());
            var entityProperties = this.entityPropertyMap[entityTypeName];

            var entity = this.destinationContext.CreateEntityInstance(entityType);

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
                    if (propertyValue != null)
                    {
                        this.destinationContext.SetProperty(entity, propertyInfo.Name, propertyValue);
                    }
                }
            }

            var entityKey = sourceObject is EntityReference ? (sourceObject as EntityReference).EntityKey : (sourceObject as EntityObject).EntityKey;
            this.entityKeyMap.Add(entityKey, entity);

            this.destinationContext.AddEntity(entity, entityTypeName);
        }

        private void CopyEntityRelations(object sourceObject, string entityTypeName, List<EntityRelation> entityRelations)
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
                            CopyManyToOneRelation(sourceObject, entityRelation.EntityProperty.Name, propertyValue);
                            break;

                        case PropertyRelationType.ReferencedByMany:
                            CopyOneToManyRelation(sourceObject, entityRelation.EntityProperty.Name, propertyValue);
                            CopyManyToManyRelation(sourceObject, entityRelation.EntityProperty.Name, propertyValue);
                            break;
                    }
                }
            }
        }

        private void CopyManyToOneRelation(object sourceObject, string propertyName, object propertyValue)
        {
            var linkedEntity = propertyValue;
            foreach (var entityProperty in GetRelationProperties(sourceObject, linkedEntity.GetType(), PropertyRelationType.ReferencedByMany))
            {
                var referencedContainer = entityProperty.EntityProperty.GetValue(linkedEntity, null) as IEnumerable<object>;
                if (referencedContainer != null)
                {
                    foreach (var entity in referencedContainer)
                    {
                        LinkEntities(entity, propertyName, linkedEntity, this.destinationContext.SetLink);
                    }
                }
            }
        }

        private void CopyOneToManyRelation(object sourceObject, string propertyName, object propertyValue)
        {
            foreach (var linkedEntity in propertyValue as IEnumerable<object>)
            {
                foreach (var entityProperty in GetRelationProperties(sourceObject, linkedEntity.GetType(), PropertyRelationType.Reference))
                {
                    var entity = entityProperty.EntityProperty.GetValue(linkedEntity, null);
                    if (entity != null)
                    {
                        LinkEntities(entity, propertyName, linkedEntity, this.destinationContext.AddLink);
                    }
                }
            }
        }

        private void CopyManyToManyRelation(object sourceObject, string propertyName, object propertyValue)
        {
            foreach (var linkedEntity in propertyValue as IEnumerable<object>)
            {
                foreach (var entityProperty in GetRelationProperties(sourceObject, linkedEntity.GetType(), PropertyRelationType.ReferencedByMany))
                {
                    var referencedContainer = entityProperty.EntityProperty.GetValue(linkedEntity, null) as IEnumerable<object>;
                    if (referencedContainer != null)
                    {
                        foreach (var entity in referencedContainer)
                        {
                            LinkEntities(entity, propertyName, linkedEntity, this.destinationContext.AddLink);
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

        private void LinkEntities(object entity, string propertyName, object linkedEntity, Action<object, string, object> linkAction)
        {
            var destinationEntity = FindEntity(entity);
            var destinationReferencedEntity = FindEntity(linkedEntity);
            if (destinationEntity == null || destinationReferencedEntity == null) return;
            linkAction(destinationEntity, propertyName, destinationReferencedEntity);
            this.destinationContext.UpdateEntity(destinationEntity);
        }

        private object FindEntity(object entity)
        {
            var entityKey = entity is EntityReference ? (entity as EntityReference).EntityKey : (entity as EntityObject).EntityKey;
            return this.entityKeyMap[entityKey];
        }

        private PropertyRelationType GetPropertyRelationType(PropertyInfo propertyInfo)
        {
            if (this.systemProperties.Contains(propertyInfo.Name))
                return PropertyRelationType.System;
            else if (propertyInfo.PropertyType.IsValueType ||
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
    }
}
