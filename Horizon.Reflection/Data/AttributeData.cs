using System;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="Attribute"/>.
    /// </summary>
    public sealed class AttributeData : MemberData
    {
        /// <summary>
        /// Cached attribute.
        /// </summary>
        private readonly object _value;

        /// <summary>
        /// Creates a new instance of <see cref="AttributeData"/>.
        /// </summary>
        /// <param name="value">Attribute value.</param>
        /// <param name="type">Attribute type.</param>
        /// <param name="declaringMember">Declaring member.</param>
        internal AttributeData(object value, Type type, MemberData declaringMember) : base(type.Name, declaringMember)
        {
            _value = value;

            DeclaringMember = declaringMember;
            Type = type.GetTypeData();
        }

        /// <summary>
        /// The <see cref="MemberData"/> that declared the current <see cref="AttributeData"/>.
        /// </summary>
        public MemberData DeclaringMember { get; }

        /// <summary>
        /// The type of the current <see cref="AttributeData"/>.
        /// </summary>
        public TypeData Type { get; }

        /// <summary>
        /// Gets the cached <see cref="Attribute"/> as the specified <see cref="TValue"/>.
        /// </summary>
        /// <param name="value">Cached attribute value.</param>
        /// <typeparam name="TValue">Attribute type.</typeparam>
        /// <returns>True if the cached attribute was retrieved; otherwise, false.</returns>
        public bool TryGetValue<TValue>(out TValue value)
        {
            if (_value is TValue temp)
            {
                value = temp;
                return true;
            }

            value = default;
            return false;
        }
    }
}