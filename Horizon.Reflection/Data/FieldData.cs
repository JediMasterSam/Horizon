using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="FieldInfo"/>.
    /// </summary>
    public sealed class FieldData : ModifierData
    {
        /// <summary>
        /// Cached <see cref="FieldInfo"/>.
        /// </summary>
        private readonly FieldInfo _fieldInfo;

        /// <summary>
        /// Collection of every <see cref="Attribute"/> applied to the current <see cref="FieldData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<AttributeData>> _attributes;

        /// <summary>
        /// Creates a new instance of <see cref="FieldData"/>.
        /// </summary>
        /// <param name="fieldInfo">Field info.</param>
        /// <param name="declaringType">Declaring type.</param>
        internal FieldData(FieldInfo fieldInfo, TypeData declaringType) : base(fieldInfo.GetModifierFlags(), fieldInfo.Name, declaringType)
        {
            _fieldInfo = fieldInfo;
            _attributes = new Lazy<IReadOnlyList<AttributeData>>(() => _fieldInfo.GetCustomAttributes(false).Select(value => new AttributeData(value, value.GetType(), this)).ToArray());

            DeclaringType = declaringType;
            FieldType = fieldInfo.FieldType.GetTypeData();
        }

        ///<inheritdoc cref="_attributes"/>
        public IReadOnlyList<AttributeData> Attributes => _attributes.Value;

        /// <summary>
        /// The declaring <see cref="TypeData"/> of the current <see cref="FieldData"/>.
        /// </summary>
        public TypeData DeclaringType { get; }

        /// <summary>
        /// The <see cref="TypeData"/> of the current <see cref="FieldData"/>.
        /// </summary>
        public TypeData FieldType { get; }

        /// <summary>
        /// Implicitly converts the specified <see cref="FieldData"/> to <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="fieldData">Field data.</param>
        /// <returns>Cached <see cref="FieldInfo"/>.</returns>
        public static implicit operator FieldInfo(FieldData fieldData)
        {
            return fieldData._fieldInfo;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="FieldData"/> equal the specified right hand side <see cref="FieldData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="FieldData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="FieldData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="FieldData"/> equals the specified right hand side <see cref="FieldData"/>; otherwise, false.</returns>
        public static bool operator ==(FieldData lhs, FieldData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Path == rhs.Path && lhs.FieldType == rhs.FieldType;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="FieldData"/> not equal the specified right hand side <see cref="FieldData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="FieldData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="FieldData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="FieldData"/> does not equal the specified right hand side <see cref="FieldData"/>; otherwise, false.</returns>
        public static bool operator !=(FieldData lhs, FieldData rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Gets the value of the current <see cref="FieldData"/> within the specified <see cref="object"/>.
        /// </summary>
        /// <param name="obj">Object that contains the current <see cref="FieldData"/>.</param>
        /// <param name="value">Field value.</param>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if both the value was retrieved and the output was cast successfully; otherwise, false.</returns>
        public bool TryGetValue<TValue>(object obj, out TValue value)
        {
            try
            {
                if (_fieldInfo.GetValue(obj) is TValue temp)
                {
                    value = temp;
                    return true;
                }

                value = default;
                return false;
            }
            catch (Exception)
            {
                value = default;
                return false;
            }
        }

        public bool TrySetValue(object obj, object value)
        {
            try
            {
                _fieldInfo.SetValue(obj, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is FieldData fieldData && this == fieldData;
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Path.GetHashCode();
                hashCode = (hashCode * 397) ^ FieldType.GetHashCode();
                return hashCode;
            }
        }
    }
}