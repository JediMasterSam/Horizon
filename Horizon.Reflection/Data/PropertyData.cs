using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class PropertyData : MemberData
    {
        private readonly PropertyInfo _propertyInfo;

        private readonly Lazy<IReadOnlyList<AttributeData>> _attributes;

        internal PropertyData(PropertyInfo propertyInfo, TypeData declaringType) : base(propertyInfo.Name, declaringType)
        {
            _propertyInfo = propertyInfo;
            _attributes = new Lazy<IReadOnlyList<AttributeData>>(() => _propertyInfo.GetCustomAttributes(false).Select(value => new AttributeData(value, value.GetType(), this)).ToArray());

            DeclaringType = declaringType;
            PropertyType = propertyInfo.PropertyType.GetTypeData();

            Get = new MethodData(propertyInfo.GetMethod, declaringType);
            Set = new MethodData(propertyInfo.SetMethod, declaringType);
        }

        public IReadOnlyList<AttributeData> Attributes => _attributes.Value;

        public TypeData DeclaringType { get; }

        public TypeData PropertyType { get; }

        public MethodData Get { get; }

        public MethodData Set { get; }

        public static implicit operator PropertyInfo(PropertyData propertyData)
        {
            return propertyData._propertyInfo;
        }
        
        public static bool operator ==(PropertyData lhs, PropertyData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Path == rhs.Path && lhs.PropertyType == rhs.PropertyType;
        }

        public static bool operator !=(PropertyData lhs, PropertyData rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return obj is PropertyData propertyData && this == propertyData;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Path.GetHashCode();
                hashCode = (hashCode * 397) ^ PropertyType.GetHashCode();
                return hashCode;
            }
        }
    }
}