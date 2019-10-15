using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class ParameterData : MemberData
    {
        private readonly ParameterInfo _parameterInfo;

        private readonly Lazy<IReadOnlyList<AttributeData>> _attributes;

        internal ParameterData(ParameterInfo parameterInfo, MethodBaseData declaringMethod) : base(parameterInfo.Name, declaringMethod)
        {
            _parameterInfo = parameterInfo;
            _attributes = new Lazy<IReadOnlyList<AttributeData>>(() => _parameterInfo.GetCustomAttributes(false).Select(value => new AttributeData(value, value.GetType(), this)).ToArray());

            DeclaringMethod = declaringMethod;
            ParameterType = parameterInfo.ParameterType.GetTypeData();
            IsOut = parameterInfo.IsOut;
            IsOptional = parameterInfo.IsOptional;
        }
        
        public IReadOnlyList<AttributeData> Attributes => _attributes.Value;

        public MethodBaseData DeclaringMethod { get; }

        public TypeData ParameterType { get; }

        public bool IsOut { get; }

        public bool IsOptional { get; }

        public static implicit operator ParameterInfo(ParameterData parameterData)
        {
            return parameterData._parameterInfo;
        }

        public static bool operator ==(ParameterData lhs, ParameterData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Path == rhs.Path && lhs.ParameterType == rhs.ParameterType && lhs.IsOut == rhs.IsOut && lhs.IsOptional == rhs.IsOptional;
        }

        public static bool operator !=(ParameterData lhs, ParameterData rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return obj is ParameterData parameterData && this == parameterData;
        }

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