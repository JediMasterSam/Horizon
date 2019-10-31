using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="ParameterInfo"/>.
    /// </summary>
    public sealed class ParameterData : MemberData
    {
        /// <summary>
        /// Cached <see cref="ParameterInfo"/>.
        /// </summary>
        private readonly ParameterInfo _parameterInfo;

        /// <summary>
        /// Collection of every <see cref="AttributeData"/> applied to the current <see cref="ParameterData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<AttributeData>> _attributes;

        /// <summary>
        /// The default value of the current <see cref="ParameterData"/>.
        /// </summary>
        private readonly Lazy<object> _defaultValue;

        /// <summary>
        /// The XML summary given to the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<string> _description;

        /// <summary>
        /// Creates a new instance of <see cref="ParameterData"/>.
        /// </summary>
        /// <param name="parameterInfo">Parameter info.</param>
        /// <param name="declaringMethod">Declaring method.</param>
        internal ParameterData(ParameterInfo parameterInfo, MethodBaseData declaringMethod) : base(parameterInfo.Name, declaringMethod)
        {
            _parameterInfo = parameterInfo;
            _attributes = new Lazy<IReadOnlyList<AttributeData>>(() => _parameterInfo.GetCustomAttributes(true).Select(value => new AttributeData(value, value.GetType(), this)).ToArray());
            _defaultValue = new Lazy<object>(() => _parameterInfo.DefaultValue);
            _description = new Lazy<string>(() => DeclaringMethod.DeclaringType.Assembly.XmlDocumentation.GetSummary(this));

            DeclaringMethod = declaringMethod;
            ParameterType = parameterInfo.ParameterType.GetTypeData();
            IsOut = parameterInfo.IsOut;
            IsOptional = parameterInfo.IsOptional;
        }

        ///<inheritdoc cref="_attributes"/>
        public override IReadOnlyList<AttributeData> Attributes => _attributes.Value;

        ///<inheritdoc cref="_defaultValue"/>
        public object DefaultValue => _defaultValue.Value;

        ///<inheritdoc cref="_description"/>
        public string Description => _description.Value;

        /// <summary>
        /// The declaring <see cref="MethodData"/> of the current <see cref="ParameterData"/>.
        /// </summary>
        public MethodBaseData DeclaringMethod { get; }

        /// <summary>
        /// The <see cref="TypeData"/> of the current <see cref="ParameterData"/>.
        /// </summary>
        public TypeData ParameterType { get; }

        /// <summary>
        /// Is the current <see cref="ParameterData"/> out?
        /// </summary>
        public bool IsOut { get; }

        /// <summary>
        /// Is the current <see cref="ParameterData"/> optional?
        /// </summary>
        public bool IsOptional { get; }

        /// <summary>
        /// Implicitly converts the specified <see cref="ParameterData"/> to <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameterData">Parameter data.</param>
        /// <returns>Cached <see cref="ParameterInfo"/>.</returns>
        public static implicit operator ParameterInfo(ParameterData parameterData)
        {
            return parameterData._parameterInfo;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="ParameterData"/> equal the specified right hand side <see cref="ParameterData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="ParameterData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="ParameterData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="ParameterData"/> equals the specified right hand side <see cref="ParameterData"/>; otherwise, false.</returns>
        public static bool operator ==(ParameterData lhs, ParameterData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Path == rhs.Path && lhs.ParameterType == rhs.ParameterType && lhs.IsOut == rhs.IsOut && lhs.IsOptional == rhs.IsOptional;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="ParameterData"/> not equal the specified right hand side <see cref="ParameterData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="ParameterData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="ParameterData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="ParameterData"/> does not equal the specified right hand side <see cref="ParameterData"/>; otherwise, false.</returns>
        public static bool operator !=(ParameterData lhs, ParameterData rhs)
        {
            return !(lhs == rhs);
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is ParameterData parameterData && this == parameterData;
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Path.GetHashCode();
                hashCode = (hashCode * 397) ^ ParameterType.GetHashCode();
                hashCode = (hashCode * 397) ^ IsOut.GetHashCode();
                hashCode = (hashCode * 397) ^ IsOptional.GetHashCode();
                return hashCode;
            }
        }
    }
}