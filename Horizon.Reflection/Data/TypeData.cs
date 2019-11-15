using System;
using System.Collections.Generic;
using System.Linq;

namespace Horizon.Reflection
{
    public sealed class TypeData : ModifierData
    {
        private readonly Type _type;

        private TypeData _genericTypeDefinition;

        private readonly Lazy<TypeData> _baseType;

        private readonly Lazy<TypeData> _declaringType;

        private readonly Lazy<IReadOnlyList<TypeData>> _interfaces;

        private readonly Lazy<IReadOnlyList<TypeData>> _genericParameterConstraints;

        internal TypeData(Type type) : base(type)
        {
            _type = type;
            _baseType = new Lazy<TypeData>(() => _type.BaseType.GetTypeData());
            _declaringType = new Lazy<TypeData>(() => _type.DeclaringType.GetTypeData());
            _interfaces = new Lazy<IReadOnlyList<TypeData>>(() => _type.GetInterfaces().Select(TypeCache.GetTypeData).ToArray());
            _genericParameterConstraints = new Lazy<IReadOnlyList<TypeData>>(() => IsGenericParameter ? _type.GetGenericParameterConstraints().Select(TypeCache.GetTypeData).ToArray() : null);

            Definition = new Definition(type);
            IsNullable = new IsNullable(type);
            IsGenericType = type.IsGenericType;
            IsGenericParameter = type.IsGenericParameter;

            Assembly = type.Assembly.GetAssemblyData();
            Fields = new FieldDataList(this);
            Constructors = new ConstructorDataList(this);
            Properties = new PropertyDataList(this);
            Methods = new MethodDataList(this);
        }

        public TypeData BaseType => _baseType.Value;

        public override TypeData DeclaringType => _declaringType.Value;

        public IReadOnlyList<TypeData> Interfaces => _interfaces.Value;

        public IReadOnlyList<TypeData> GenericParameterConstraints => _genericParameterConstraints.Value;

        public AssemblyData Assembly { get; }

        public FieldDataList Fields { get; }

        public ConstructorDataList Constructors { get; }

        public PropertyDataList Properties { get; }

        public MethodDataList Methods { get; }

        public Definition Definition { get; }

        public bool IsNullable { get; }

        public bool IsGenericType { get; }

        public bool IsGenericParameter { get; }

        public TypeData GenericTypeDefinition
        {
            get => _genericTypeDefinition ??= _type.GetGenericTypeDefinition().GetTypeData();
            set => _genericTypeDefinition = value;
        }

        public static implicit operator Type(TypeData typeData)
        {
            return typeData._type;
        }

        public bool IsAssignableTo(TypeData typeData)
        {
            return typeData != null && (Equals(typeData) || Implements(typeData) || Extends(typeData));
        }

        public bool Implements(TypeData typeData)
        {
            return typeData != null && !(typeData.Definition.Flags & DefinitionFlags.Interface) && Interfaces.Any(interfaceType => interfaceType.Equals(typeData));
        }

        public bool Extends(TypeData typeData)
        {
            if (typeData == null || !(typeData.Definition.Flags & DefinitionFlags.Class)) return false;

            var baseType = BaseType;

            while (baseType != null)
            {
                if (baseType.Equals(typeData))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }

        public TypeData MakeGenericType(params Type[] typeArguments)
        {
            var type = _type.MakeGenericType(typeArguments).GetTypeData();
            type.GenericTypeDefinition = this;
            return type;
        }

        public TValue? StackAlloc<TValue>(params object[] parameters) where TValue : struct
        {
            if (!(Definition.Flags | DefinitionFlags.Value)) return null;

            if (parameters.Length == 0)
            {
                return (TValue) Activator.CreateInstance(_type);
            }

            foreach (var constructor in Constructors)
            {
                if (constructor.TryInvoke<TValue>(parameters, out var value))
                {
                    return value;
                }
            }

            return null;
        }

        public TValue HeapAlloc<TValue>(params object[] parameters) where TValue : class
        {
            const ModifierFlags invalidFlags = ModifierFlags.Abstract | ModifierFlags.Static;
            
            if (!(Definition.Flags & DefinitionFlags.Class) || Modifier.Flags | invalidFlags) return null;

            foreach (var constructor in Constructors)
            {
                if (constructor.TryInvoke<TValue>(parameters, out var value))
                {
                    return value;
                }
            }

            return null;
        }
    }
}