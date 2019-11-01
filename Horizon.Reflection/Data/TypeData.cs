using System;
using System.Collections.Generic;
using System.Linq;
using Horizon.Numerics;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="Type"/>.
    /// </summary>
    public sealed class TypeData : ModifierData
    {
        /// <summary>
        /// Cached <see cref="Type"/>.
        /// </summary>
        private readonly Type _type;

        /// <summary>
        /// The <see cref="DefinitionFlags"/> for the current <see cref="TypeData"/>.
        /// </summary>
        private readonly BitField<DefinitionFlags> _definitionFlags;

        /// <summary>
        /// The declaring <see cref="AssemblyData"/> of the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<AssemblyData> _assembly;

        /// <summary>
        /// The base <see cref="TypeData"/> of the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<TypeData> _baseType;

        /// <summary>
        /// The declaring <see cref="TypeData"/> of the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<TypeData> _declaringType;

        /// <summary>
        /// Collection of every <see cref="TypeData"/> that the current <see cref="TypeData"/> implements.
        /// </summary>
        private readonly Lazy<IReadOnlyList<TypeData>> _interfaces;

        /// <summary>
        /// Collection of every <see cref="GenericArgumentData"/> that the current <see cref="TypeData"/> defines.
        /// </summary>
        private readonly Lazy<IReadOnlyList<GenericArgumentData>> _genericArguments;

        /// <summary>
        /// Collection of every <see cref="AttributeData"/> applied to the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<AttributeData>> _attributes;

        /// <summary>
        /// Collection of every <see cref="FieldData"/> within the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<FieldData>> _fields;

        /// <summary>
        /// Collection of every <see cref="PropertyData"/> within the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<PropertyData>> _properties;

        /// <summary>
        /// Collection of every <see cref="MethodData"/> within the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<MethodData>> _methods;

        /// <summary>
        /// Collection of every <see cref="ConstructorData"/> within the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<ConstructorData>> _constructors;

        /// <summary>
        /// The XML summary given to the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<string> _description;

        /// <summary>
        /// Creates a new instance of <see cref="TypeData"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        internal TypeData(Type type) : base(type.GetModifierFlags(), type.Name, type.FullName)
        {
            _type = type;
            _definitionFlags = type.GetDefinitionFlags();
            _assembly = new Lazy<AssemblyData>(() => _type.Assembly.GetAssemblyData());
            _baseType = new Lazy<TypeData>(() => _type.BaseType.GetTypeData());
            _declaringType = new Lazy<TypeData>(() => _type.DeclaringType.GetTypeData());
            _interfaces = new Lazy<IReadOnlyList<TypeData>>(() => _type.GetInterfaces().Select(interfaceType => interfaceType.GetTypeData()).ToArray());
            _genericArguments = new Lazy<IReadOnlyList<GenericArgumentData>>(() => _type.GetGenericArguments().Select(genericArgumentType => new GenericArgumentData(genericArgumentType, this)).ToArray());
            _attributes = new Lazy<IReadOnlyList<AttributeData>>(() => _type.GetCustomAttributes(true).Select(value => new AttributeData(value, value.GetType(), this)).ToArray());
            _fields = new Lazy<IReadOnlyList<FieldData>>(() => FieldDataFactory.Instance.Get(this));
            _properties = new Lazy<IReadOnlyList<PropertyData>>(() => PropertyDataFactory.Instance.Get(this));
            _methods = new Lazy<IReadOnlyList<MethodData>>(() => MethodDataFactory.Instance.Get(this));
            _constructors = new Lazy<IReadOnlyList<ConstructorData>>(() => ConstructorDataFactory.Instance.Get(this));
            _description = new Lazy<string>(() => Assembly.XmlDocumentation.GetSummary(this));
        }

        ///<inheritdoc cref="_assembly"/>
        public AssemblyData Assembly => _assembly.Value;

        ///<inheritdoc cref="_baseType"/>
        public TypeData BaseType => _baseType.Value;

        ///<inheritdoc cref="_declaringType"/>
        public TypeData DeclaringType => _declaringType.Value;

        ///<inheritdoc cref="_interfaces"/>
        public IReadOnlyList<TypeData> Interfaces => _interfaces.Value;

        ///<inheritdoc cref="_genericArguments"/>
        public IReadOnlyList<GenericArgumentData> GenericArguments => _genericArguments.Value;

        ///<inheritdoc cref="_attributes"/>
        public override IReadOnlyList<AttributeData> Attributes => _attributes.Value;

        ///<inheritdoc cref="_fields"/>
        public IReadOnlyList<FieldData> Fields => _fields.Value;

        ///<inheritdoc cref="_properties"/>
        public IReadOnlyList<PropertyData> Properties => _properties.Value;

        ///<inheritdoc cref="_methods"/>
        public IReadOnlyList<MethodData> Methods => _methods.Value;

        ///<inheritdoc cref="_constructors"/>
        public IReadOnlyCollection<ConstructorData> Constructors => _constructors.Value;

        ///<inheritdoc cref="_description"/>
        public string Description => _description.Value;

        /// <summary>
        /// Implicitly converts the specified <see cref="TypeData"/> to <see cref="Type"/>.
        /// </summary>
        /// <param name="typeData">Type data.</param>
        /// <returns>Cached <see cref="Type"/>.</returns>
        public static implicit operator Type(TypeData typeData)
        {
            return typeData._type;
        }

        /// <summary>
        /// Does the left hand side <see cref="TypeData"/> contain any of the set bits from the specified right hand side <see cref="DefinitionFlags"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="TypeData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="DefinitionFlags"/>.</param>
        /// <returns>True if the specified left hand side <see cref="TypeData"/> contains any of the set bits in the specified right hand side <see cref="DefinitionFlags"/>; otherwise, false.</returns>
        public static bool operator |(TypeData lhs, DefinitionFlags rhs)
        {
            return lhs != null && lhs._definitionFlags | rhs;
        }

        /// <summary>
        /// Does the left hand side <see cref="TypeData"/> contain all of the set bits from the specified right hand side <see cref="DefinitionFlags"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="TypeData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="DefinitionFlags"/>.</param>
        /// <returns>True if the specified left hand side <see cref="TypeData"/> contains all of the set bits in the specified right hand side <see cref="DefinitionFlags"/>; otherwise, false.</returns>
        public static bool operator &(TypeData lhs, DefinitionFlags rhs)
        {
            return lhs != null && lhs._definitionFlags & rhs;
        }

        /// <summary>
        /// Is the current <see cref="TypeData"/> assignable to the specified <see cref="TypeData"/>?
        /// </summary>
        /// <param name="typeData">Type data.</param>
        /// <returns>True if the current <see cref="TypeData"/> is assignable to the specified <see cref="TypeData"/>; otherwise, false.</returns>
        public bool IsAssignableTo(TypeData typeData)
        {
            return typeData == this || Extends(typeData) || Implements(typeData);
        }

        /// <summary>
        /// Does the current <see cref="TypeData"/> extend the specified <see cref="TypeData"/>?
        /// </summary>
        /// <param name="baseTypeData">Base type data.</param>
        /// <returns>True if the current <see cref="TypeData"/> extends the specified <see cref="TypeData"/>; otherwise, false.</returns>
        public bool Extends(TypeData baseTypeData)
        {
            if (!(baseTypeData & DefinitionFlags.Class))
            {
                return false;
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

        /// <summary>
        /// Does the current <see cref="TypeData"/> implement the specified <see cref="TypeData"/>?
        /// </summary>
        /// <param name="interfaceTypeData">Interface type data.</param>
        /// <returns>True if the current <see cref="TypeData"/> implements the specified <see cref="TypeData"/>; otherwise, false.</returns>
        public bool Implements(TypeData interfaceTypeData)
        {
            if (!(interfaceTypeData & DefinitionFlags.Interface))
            {
                return false;
            }

            return Interfaces.Any(typeData => typeData == interfaceTypeData);
        }

        /// <summary>
        /// Creates a new instance of the current <see cref="TypeData"/>.
        /// </summary>
        /// <param name="parameters">Constructor parameters.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if both a constructor was invoked and the output was cast successfully; otherwise, false.</returns>
        public bool TryCreate<TValue>(object[] parameters, out TValue value)
        {
            if (this & ModifierFlags.Abstract || this | (DefinitionFlags.Interface | DefinitionFlags.DeconstructedGeneric))
            {
                value = default;
                return false;
            }

            if (Constructors.Count == 0)
            {
                if (parameters == null)
                {
                    value = Activator.CreateInstance<TValue>();
                    return true;
                }

                value = default;
                return false;
            }

            foreach (var constructorData in Constructors)
            {
                if (!constructorData.TryInvoke(parameters, out TValue temp)) continue;
                value = temp;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Constructs a generic type out of the current <see cref="TypeData"/>.
        /// </summary>
        /// <param name="typeArguments">Type arguments.</param>
        /// <param name="genericType">Generic type.</param>
        /// <returns>True if a generic type was successfully created; otherwise, false.</returns>
        public bool TryMakeGenericType(Type[] typeArguments, out TypeData genericType)
        {
            if (!(this & DefinitionFlags.DeconstructedGeneric))
            {
                genericType = null;
                return false;
            }

            try
            {
                genericType = _type.MakeGenericType(typeArguments).GetTypeData();
                return true;
            }
            catch (Exception)
            {
                genericType = null;
                return false;
            }
        }

//        public bool TryGetMethod(MethodBaseDataQuery query, out MethodBaseData method)
//        {
//            method = query.Name == ".ctor" ? Select(Constructors) : Select(Methods);
//
//            return method != null;
//
//            MethodBaseData Select<TMethodBaseData>(IEnumerable<TMethodBaseData> methods) where TMethodBaseData : MethodBaseData
//            {
//                return methods.Where(methodData => methodData.Name == query.Name && methodData.Parameters.Count == query.Parameters.Count)
//                    .FirstOrDefault(methodData => !methodData.Parameters.Where((parameter, index) => parameter.ParameterType != query.Parameters[index]).Any());
//            }
//        }
    }
}