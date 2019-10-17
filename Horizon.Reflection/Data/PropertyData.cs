using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="PropertyInfo"/>.
    /// </summary>
    public sealed class PropertyData : MemberData
    {
        /// <summary>
        /// Cached <see cref="PropertyInfo"/>.
        /// </summary>
        private readonly PropertyInfo _propertyInfo;

        /// <summary>
        /// Collection of every <see cref="AttributeData"/> applied to the current <see cref="PropertyData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<AttributeData>> _attributes;

        /// <summary>
        /// Creates a new instance of <see cref="PropertyData"/>.
        /// </summary>
        /// <param name="propertyInfo">Property info.</param>
        /// <param name="declaringType">Declaring type.</param>
        internal PropertyData(PropertyInfo propertyInfo, TypeData declaringType) : base(propertyInfo.Name, declaringType)
        {
            _propertyInfo = propertyInfo;
            _attributes = new Lazy<IReadOnlyList<AttributeData>>(() => _propertyInfo.GetCustomAttributes(false).Select(value => new AttributeData(value, value.GetType(), this)).ToArray());

            DeclaringType = declaringType;
            PropertyType = propertyInfo.PropertyType.GetTypeData();

            Get = propertyInfo.CanRead ? new MethodData(propertyInfo.GetMethod, declaringType) : null;
            Set = propertyInfo.CanWrite ? new MethodData(propertyInfo.SetMethod, declaringType) : null;
        }
        
        ///<inheritdoc cref="_attributes"/>
        public IReadOnlyList<AttributeData> Attributes => _attributes.Value;

        /// <summary>
        /// The declaring <see cref="TypeData"/> of the current <see cref="PropertyData"/>.
        /// </summary>
        public TypeData DeclaringType { get; }

        /// <summary>
        /// The <see cref="TypeData"/> of the current <see cref="PropertyData"/>.
        /// </summary>
        public TypeData PropertyType { get; }

        /// <summary>
        /// The get accessor of the current <see cref="PropertyData"/>.
        /// </summary>
        public MethodData Get { get; }

        /// <summary>
        /// The set accessor of the current <see cref="PropertyData"/>.
        /// </summary>
        public MethodData Set { get; }

        /// <summary>
        /// Implicitly converts the specified <see cref="PropertyData"/> to <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyData">Property data.</param>
        /// <returns>Cached <see cref="PropertyInfo"/>.</returns>
        public static implicit operator PropertyInfo(PropertyData propertyData)
        {
            return propertyData._propertyInfo;
        }
        
        /// <summary>
        /// Does the specified left hand side <see cref="PropertyData"/> equal the specified right hand side <see cref="PropertyData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="PropertyData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="PropertyData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="PropertyData"/> equals the specified right hand side <see cref="PropertyData"/>; otherwise, false.</returns>
        public static bool operator ==(PropertyData lhs, PropertyData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Path == rhs.Path && lhs.PropertyType == rhs.PropertyType;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="PropertyData"/> not equal the specified right hand side <see cref="PropertyData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="PropertyData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="PropertyData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="PropertyData"/> does not equal the specified right hand side <see cref="PropertyData"/>; otherwise, false.</returns>
        public static bool operator !=(PropertyData lhs, PropertyData rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Gets the value of the current <see cref="PropertyData"/> within the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj">Object that contains the current <see cref="PropertyData"/>.</param>
        /// <param name="value">Property value.</param>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if both the value was retrieved and the output was cast successfully; otherwise, false.</returns>
        public bool TryGetValue<TValue>(object obj, out TValue value)
        {
            if (Get != null)
            {
                return Get.TryInvoke(obj, null, out value);
            }
            
            value = default;
            return false;
        }

        /// <summary>
        /// Sets the value of the current <see cref="PropertyData"/> within the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj">Object that contains the current <see cref="PropertyData"/>.</param>
        /// <param name="value">Property value.</param>
        /// <returns>True if the value was set successfully; otherwise, false.</returns>
        public bool TrySetValue<TValue>(object obj, object value)
        {
            return Set != null && Set.TryInvoke(obj, new[] {value});
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is PropertyData propertyData && this == propertyData;
        }

        ///<inheritdoc/>
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