using System;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="ConstructorInfo"/>.
    /// </summary>
    public sealed class ConstructorData : MethodBaseData
    {
        /// <summary>
        /// Cached <see cref="ConstructorInfo"/>.
        /// </summary>
        private readonly ConstructorInfo _constructorInfo;

        /// <summary>
        /// Creates a new instance of <see cref="ConstructorData"/>.
        /// </summary>
        /// <param name="constructorInfo">Constructor info.</param>
        /// <param name="declaringType">Declaring type.</param>
        internal ConstructorData(ConstructorInfo constructorInfo, TypeData declaringType) : base(constructorInfo, declaringType)
        {
            _constructorInfo = constructorInfo;

            IsDefault = Parameters.Count == 0;
        }

        /// <summary>
        /// Is the current <see cref="ConstructorData"/> parameterless?
        /// </summary>
        public bool IsDefault { get; }

        /// <summary>
        /// Implicitly converts the specified <see cref="ConstructorData"/> to <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="constructorData">Constructor data.</param>
        /// <returns>Cached <see cref="ConstructorInfo"/>.</returns>
        public static implicit operator ConstructorInfo(ConstructorData constructorData)
        {
            return constructorData._constructorInfo;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="ConstructorData"/> equal the specified right hand side <see cref="ConstructorData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="ConstructorData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="ConstructorData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="ConstructorData"/> equals the specified right hand side <see cref="ConstructorData"/>; otherwise, false.</returns>
        public static bool operator ==(ConstructorData lhs, ConstructorData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return Equals(lhs, rhs);
        }

        /// <summary>
        /// Does the specified left hand side <see cref="ConstructorData"/> not equal the specified right hand side <see cref="ConstructorData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="ConstructorData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="ConstructorData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="ConstructorData"/> does not equal the specified right hand side <see cref="ConstructorData"/>; otherwise, false.</returns>
        public static bool operator !=(ConstructorData lhs, ConstructorData rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Invokes the cached <see cref="ConstructorInfo"/> in the current <see cref="ConstructorData"/> with the specified parameters.
        /// </summary>
        /// <param name="parameters">Constructor parameters.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if both the constructor was invoked and the output was cast successfully; otherwise, false.</returns>
        public bool TryInvoke<TValue>(object[] parameters, out TValue value)
        {
            try
            {
                if (_constructorInfo.Invoke(parameters) is TValue temp)
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

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is ConstructorData constructorData && this == constructorData;
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Path.GetHashCode();
                hashCode = (hashCode * 397) ^ (Parameters != null ? Parameters.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}