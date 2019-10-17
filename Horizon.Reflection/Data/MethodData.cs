using System;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="MethodInfo"/>.
    /// </summary>
    public sealed class MethodData : MethodBaseData
    {
        /// <summary>
        /// Cached <see cref="MethodInfo"/>.
        /// </summary>
        private readonly MethodInfo _methodInfo;

        /// <summary>
        /// Creates a new instance of <see cref="MethodData"/>.
        /// </summary>
        /// <param name="methodInfo">Method info.</param>
        /// <param name="declaringType">Declaring type.</param>
        internal MethodData(MethodInfo methodInfo, TypeData declaringType) : base(methodInfo, declaringType)
        {
            _methodInfo = methodInfo;

            ReturnType = methodInfo.ReturnType.GetTypeData();
            IsVoid = ReturnType == typeof(void).GetTypeData();
        }

        /// <summary>
        /// The return <see cref="TypeData"/> of the current <see cref="MethodData"/>.
        /// </summary>
        public TypeData ReturnType { get; }

        /// <summary>
        /// Is the <see cref="ReturnType"/> equal to <see cref="Void"/>?
        /// </summary>
        private bool IsVoid { get; }

        /// <summary>
        /// Implicitly converts the specified <see cref="MethodData"/> to <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="methodData">Method data.</param>
        /// <returns>Cached <see cref="MethodInfo"/>.</returns>
        public static implicit operator MethodInfo(MethodData methodData)
        {
            return methodData._methodInfo;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="MethodData"/> equal the specified right hand side <see cref="MethodData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="MethodData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="MethodData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="MethodData"/> equals the specified right hand side <see cref="MethodData"/>; otherwise, false.</returns>
        public static bool operator ==(MethodData lhs, MethodData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return Equals(lhs, rhs) && lhs.ReturnType == rhs.ReturnType;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="MethodData"/> not equal the specified right hand side <see cref="MethodData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="MethodData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="MethodData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="MethodData"/> does not equal the specified right hand side <see cref="MethodData"/>; otherwise, false.</returns>
        public static bool operator !=(MethodData lhs, MethodData rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Invokes the cached <see cref="MethodInfo"/> in the current <see cref="MethodData"/> with the specified parameters on the specified object.
        /// </summary>
        /// <param name="obj">The object on which to invoke the method.</param>
        /// <param name="parameters">Method parameters.</param>
        /// <returns>True if the method was invoked successfully; otherwise, false.</returns>
        public bool TryInvoke(object obj, object[] parameters)
        {
            try
            {
                _methodInfo.Invoke(obj, parameters);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Invokes the cached <see cref="MethodInfo"/> in the current <see cref="MethodData"/> with the specified parameters on the specified object.
        /// </summary>
        /// <param name="obj">The object on which to invoke the method.</param>
        /// <param name="parameters">Method parameters.</param>
        /// <param name="value">Returned value.</param>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if both the method was invoked and the output was cast successfully; otherwise, false.</returns>
        public bool TryInvoke<TValue>(object obj, object[] parameters, out TValue value)
        {
            try
            {
                if (!IsVoid && _methodInfo.Invoke(obj, parameters) is TValue temp)
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
            return obj is MethodData methodData && this == methodData;
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Path.GetHashCode();
                hashCode = (hashCode * 397) ^ (ReturnType != null ? ReturnType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Parameters != null ? Parameters.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}