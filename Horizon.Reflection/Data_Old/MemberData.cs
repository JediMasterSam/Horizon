using System.Collections.Generic;
using System.Linq;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents the member information of a reflected value.
    /// </summary>
    public abstract class MemberData
    {
        /// <summary>
        /// Creates a new instance of <see cref="MemberData"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="path">Path.</param>
        protected internal MemberData(string name, string path)
        {
            Name = name;
            Path = path;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MemberData"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="declaringMember">Declaring member.</param>
        protected internal MemberData(string name, MemberData declaringMember)
        {
            Name = name;
            Path = $"{declaringMember.Path}.{name}";
        }

        /// <summary>
        /// The name of the current <see cref="MemberData"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The path to the current <see cref="MemberData"/>.
        /// </summary>
        public string Path { get; }
        
        /// <summary>
        /// Collection of every <see cref="AttributeData"/> applied to the current <see cref="MemberData"/>.
        /// </summary>
        public abstract IReadOnlyList<AttributeData> Attributes { get; }

        /// <summary>
        /// Does the specified left hand side <see cref="MemberData"/> equal the specified right hand side <see cref="MemberData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="MemberData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="MemberData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="MemberData"/> equals the specified right hand side <see cref="MemberData"/>; otherwise, false.</returns>
        public static bool operator ==(MemberData lhs, MemberData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Path == rhs.Path;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="MemberData"/> not equal the specified right hand side <see cref="MemberData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="MemberData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="MemberData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="MemberData"/> does not equal the specified right hand side <see cref="MemberData"/>; otherwise, false.</returns>
        public static bool operator !=(MemberData lhs, MemberData rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Gets all attributes applied to the current <see cref="MemberData"/> that are assignable from <see cref="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>Collection of attributes assignable from <see cref="TValue"/>.</returns>
        public IEnumerable<TValue> GetAttributes<TValue>()
        {
            foreach (var attribute in Attributes)
            {
                if (attribute.TryGetValue<TValue>(out var value))
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Does the current <see cref="MemberData"/> have an applied attribute assignable from <see cref="TValue"/>?
        /// </summary>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if the current <see cref="MemberData"/> has an attribute assignable from <see cref="TValue"/>; otherwise, false.</returns>
        public bool HasAttribute<TValue>()
        {
            return GetAttributes<TValue>().Any();
        }

        /// <summary>
        /// Gets the first attribute applied to the current <see cref="MemberData"/> that is assignable from <see cref="TValue"/>.
        /// </summary>
        /// <param name="value">Attribute value.</param>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if there is an applied attribute to the current <see cref="MemberData"/> that is assignable from <see cref="TValue"/>; otherwise, false.</returns>
        public bool TryGetAttribute<TValue>(out TValue value)
        {
            foreach (var attribute in Attributes)
            {
                if (attribute.TryGetValue(out value))
                {
                    return true;
                }
            }

            value = default;
            return false;
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is MemberData memberData && this == memberData;
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            return Path;
        }
    }
}