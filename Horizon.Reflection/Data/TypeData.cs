using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Horizon.Numerics;

namespace Horizon.Reflection
{
    public sealed class TypeData : ModifierData
    {
        private readonly Type _type;

        private readonly BitField<DefinitionFlags> _definitionFlags;

        private readonly Lazy<AssemblyData> _assembly;

        private readonly Lazy<TypeData> _baseType;

        private readonly Lazy<TypeData> _declaringType;

        private readonly Lazy<IReadOnlyList<TypeData>> _interfaces;

        private readonly Lazy<IReadOnlyList<AttributeData>> _attributes;

        private readonly Lazy<IReadOnlyList<FieldData>> _fields;

        private readonly Lazy<IReadOnlyList<PropertyData>> _properties;

        private readonly Lazy<IReadOnlyList<MethodData>> _methods;

        private readonly Lazy<IReadOnlyList<ConstructorData>> _constructors;

        internal TypeData(Type type) : base(type.GetModifierFlags(), type.Name, type.FullName)
        {
            _type = type;
            _definitionFlags = type.GetDefinitionFlags();
            _assembly = new Lazy<AssemblyData>(() => _type.Assembly.GetAssemblyData());
            _baseType = new Lazy<TypeData>(() => _type.BaseType.GetTypeData());
            _declaringType = new Lazy<TypeData>(() => _type.DeclaringType.GetTypeData());
            _interfaces = new Lazy<IReadOnlyList<TypeData>>(() => _type.GetInterfaces().Select(interfaceType => interfaceType.GetTypeData()).ToArray());
            _attributes = new Lazy<IReadOnlyList<AttributeData>>(() => _type.GetCustomAttributes(false).Select(value => new AttributeData(value, value.GetType(), this)).ToArray());
            _fields = new Lazy<IReadOnlyList<FieldData>>(() => FieldDataFactory.Instance.Get(this));
            _properties = new Lazy<IReadOnlyList<PropertyData>>(() => PropertyDataFactory.Instance.Get(this));
            _methods = new Lazy<IReadOnlyList<MethodData>>(() => MethodDataFactory.Instance.Get(this));
            _constructors = new Lazy<IReadOnlyList<ConstructorData>>(() => ConstructorDataFactory.Instance.Get(this));
        }

        public AssemblyData Assembly => _assembly.Value;

        public TypeData BaseType => _baseType.Value;

        public TypeData DeclaringType => _declaringType.Value;

        public IReadOnlyList<TypeData> Interfaces => _interfaces.Value;

        public IReadOnlyList<AttributeData> Attributes => _attributes.Value;

        public IReadOnlyList<FieldData> Fields => _fields.Value;

        public IReadOnlyList<PropertyData> Properties => _properties.Value;

        public IReadOnlyList<MethodData> Methods => _methods.Value;

        public IReadOnlyCollection<ConstructorData> Constructors => _constructors.Value;

        public static implicit operator Type(TypeData typeData)
        {
            return typeData._type;
        }

        public static bool operator |(TypeData lhs, DefinitionFlags rhs)
        {
            return lhs != null && lhs._definitionFlags | rhs;
        }

        public static bool operator &(TypeData lhs, DefinitionFlags rhs)
        {
            return lhs != null && lhs._definitionFlags & rhs;
        }

        public bool IsAssignableTo(TypeData typeData)
        {
            if (typeData == this)
            {
                return true;
            }

            if (typeData & DefinitionFlags.Class)
            {
                return Extends(typeData);
            }

            if (typeData & DefinitionFlags.Interface)
            {
                return Implements(typeData);
            }

            return false;
        }

        public bool Extends(TypeData baseTypeData)
        {
            if (!(baseTypeData & DefinitionFlags.Class))
            {
                throw new ArgumentException($"{baseTypeData.Path} is not a class.");
            }

            var typeData = BaseType;

            while (typeData != null)
            {
                if (typeData == baseTypeData)
                {
                    return true;
                }

                typeData = typeData.BaseType;
            }

            return false;
        }

        public bool Implements(TypeData interfaceTypeData)
        {
            if (!(interfaceTypeData & DefinitionFlags.Interface))
            {
                throw new ArgumentException($"{interfaceTypeData.Path} is not an interface.");
            }

            return Interfaces.Any(typeData => typeData == interfaceTypeData);
        }

        public bool TryCreate<TValue>(object[] parameters, out TValue value)
        {
            foreach (var constructorData in Constructors)
            {
                if (!constructorData.TryInvoke(parameters, out TValue temp)) continue;
                value = temp;
                return true;
            }

            value = default;
            return false;
        }
    }
}