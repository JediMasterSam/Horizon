using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Horizon.OData.Attributes;
using Horizon.Reflection;
using Microsoft.AspNet.OData.Builder;
using Newtonsoft.Json;

namespace Horizon.OData.Builders
{
    public sealed class EdmTypeBuilder
    {
        private static readonly TypeData EnumerableType = typeof(IEnumerable).GetTypeData();

        private readonly ODataModelBuilder _oDataModelBuilder;

        private readonly Dictionary<TypeData, IEdmTypeConfiguration> _edmTypeConfigurations;

        private IReadOnlyList<TypeData> _ignoredAttributes;

        public IReadOnlyList<TypeData> IgnoreAttributes
        {
            get => _ignoredAttributes;
            set => _ignoredAttributes = value ?? new TypeData[0];
        }

        public EdmTypeBuilder(ODataModelBuilder oDataModelBuilder)
        {
            _oDataModelBuilder = oDataModelBuilder;
            _edmTypeConfigurations = new Dictionary<TypeData, IEdmTypeConfiguration>();
            _ignoredAttributes = new[] {typeof(JsonIgnoreAttribute).GetTypeData()};
        }

        private static void SetQueryConfigurations(PropertyData property, EntityTypeConfiguration entityType)
        {
            var properties = new[] {property.Name};

            if (property.TryGetAttribute<ExpandAttribute>(out var expandAttribute))
            {
                entityType.QueryConfiguration.SetExpand(properties, expandAttribute.MaxDepth, expandAttribute.SelectExpandType);
            }

            if (property.TryGetAttribute<FilterAttribute>(out var filterAttribute))
            {
                entityType.QueryConfiguration.SetFilter(properties, filterAttribute.EnableFilter);
            }

            if (property.TryGetAttribute<SelectAttribute>(out var selectAttribute))
            {
                entityType.QueryConfiguration.SetSelect(properties, selectAttribute.SelectExpandType);
            }

            if (property.TryGetAttribute<OrderByAttribute>(out var orderByAttribute))
            {
                entityType.QueryConfiguration.SetFilter(properties, orderByAttribute.EnableOrderBy);
            }
        }

        public bool TryBuild(TypeData type, out IEdmTypeConfiguration edmType)
        {
            edmType = Build(type);
            return edmType != null;
        }

        private IEdmTypeConfiguration Build(TypeData type)
        {
            if (AddComplexType(type, out var complexType))
            {
                return complexType;
            }

            if (AddEntityType(type, out var entityType))
            {
                return entityType;
            }

            return AddEnumType(type, out var enumType) ? enumType : null;
        }

        private bool AddComplexType(TypeData type, out ComplexTypeConfiguration complexType)
        {
            if (!(type | DefinitionFlags.Class) || type.Implements(EnumerableType))
            {
                complexType = null;
                return false;
            }

            if (!TryAdd(type, _oDataModelBuilder.AddComplexType, out complexType)) return false;

            RegisterProperties(type, complexType, null);
            return true;
        }

        private bool AddEntityType(TypeData type, out EntityTypeConfiguration entityType)
        {
            if (!type.TryGetAttribute<EntityTypeAttribute>(out var entityTypeAttribute))
            {
                entityType = null;
                return false;
            }

            if (!TryAdd(type, _oDataModelBuilder.AddEntityType, out entityType)) return false;

            RegisterProperties(type, entityType, SetQueryConfigurations);

            entityType.QueryConfiguration.SetCount(entityTypeAttribute.EnableCount);
            entityType.QueryConfiguration.SetMaxTop(entityTypeAttribute.MaxTop);
            entityType.QueryConfiguration.SetPageSize(entityTypeAttribute.PageSize);

            return true;
        }

        private bool AddEnumType(TypeData type, out EnumTypeConfiguration enumType)
        {
            if (type | DefinitionFlags.Enum) return TryAdd(type, _oDataModelBuilder.AddEnumType, out enumType);

            enumType = null;
            return false;
        }

        private bool TryAdd<TConfiguration>(TypeData type, Func<Type, TConfiguration> builder, out TConfiguration configuration) where TConfiguration : IEdmTypeConfiguration
        {
            if (_edmTypeConfigurations.TryGetValue(type, out var edmType) && edmType is TConfiguration)
            {
                configuration = default;
                return false;
            }

            _edmTypeConfigurations[type] = configuration = builder.Invoke(type);
            return true;
        }

        private void RegisterProperties<TConfiguration>(TypeData type, TConfiguration configuration, Action<PropertyData, TConfiguration> onRegister) where TConfiguration : StructuralTypeConfiguration
        {
            foreach (var genericArgument in type.GenericArguments)
            {
                Build(genericArgument.GenericArgumentType);
            }

            foreach (var property in type.Properties)
            {
                if (property.Get == null || property.Set == null || property.Attributes.Any(attribute => IgnoreAttributes.Contains(attribute.Type)))
                {
                    configuration.RemoveProperty(property);
                }
                else
                {
                    onRegister?.Invoke(property, configuration);
                    Build(property.PropertyType);
                }
            }
        }
    }
}