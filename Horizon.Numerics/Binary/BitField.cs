using System;

namespace Horizon.Numerics
{
    /// <summary>
    /// Represents a sequence of bits.
    /// </summary>
    /// <typeparam name="T">Enum type.</typeparam>
    public readonly struct BitField<T> where T : Enum
    {
        /// <summary>
        /// The sequence of bits for the current <see cref="BitField{T}"/>.
        /// </summary>
        private readonly long _bits;

        /// <summary>
        /// Creates a new instance of <see cref="BitField{T}"/>.
        /// </summary>
        /// <param name="value"><see cref="Enum"/> that can be treated as a bit field.</param>
        private BitField(T value)
        {
            _bits = Convert.ToInt64(value);
        }

        /// <summary>
        /// Implicitly converts the specified <see cref="Enum"/> to a <see cref="BitField{T}"/>.
        /// </summary>
        /// <param name="value"><see cref="Enum"/> that can be treated as a bit field.</param>
        /// <returns>A <see cref="BitField{T}"/> that represents the specified <see cref="Enum"/>.</returns>
        public static implicit operator BitField<T>(T value)
        {
            return new BitField<T>(value);
        }

        /// <summary>
        /// Does the left hand side <see cref="BitField{T}"/> equal the right hand side <see cref="BitField{T}"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="BitField{T}"/>.</param>
        /// <param name="rhs">Right hand side <see cref="BitField{T}"/>.</param>
        /// <returns>True if the specified left hand side <see cref="BitField{T}"/> does equal the specified right hand side <see cref="BitField{T}"/>; otherwise, false.</returns>
        public static bool operator ==(BitField<T> lhs, BitField<T> rhs)
        {
            return lhs._bits == rhs._bits;
        }

        /// <summary>
        /// Does the left hand side <see cref="BitField{T}"/> not equal the right hand side <see cref="BitField{T}"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="BitField{T}"/>.</param>
        /// <param name="rhs">Right hand side <see cref="BitField{T}"/>.</param>
        /// <returns>True if the specified left hand side <see cref="BitField{T}"/> does not equal the specified right hand side <see cref="BitField{T}"/>; otherwise, false.</returns>
        public static bool operator !=(BitField<T> lhs, BitField<T> rhs)
        {
            return lhs._bits != rhs._bits;
        }

        /// <summary>
        /// Does the left hand side <see cref="BitField{T}"/> contain any of the set bits from the specified right hand side <see cref="BitField{T}"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="BitField{T}"/>.</param>
        /// <param name="rhs">Right hand side <see cref="BitField{T}"/>.</param>
        /// <returns>True if the specified left hand side <see cref="BitField{T}"/> contains any of the set bits in the specified right hand side <see cref="BitField{T}"/>; otherwise, false.</returns>
        public static bool operator |(BitField<T> lhs, BitField<T> rhs)
        {
            return (lhs._bits & rhs._bits) != 0;
        }

        /// <summary>
        /// Does the left hand side <see cref="BitField{T}"/> contain all of the set bits from the specified right hand side <see cref="BitField{T}"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="BitField{T}"/>.</param>
        /// <param name="rhs">Right hand side <see cref="BitField{T}"/>.</param>
        /// <returns>True if the specified left hand side <see cref="BitField{T}"/> contains all of the set bits in the specified right hand side <see cref="BitField{T}"/>; otherwise, false.</returns>
        public static bool operator &(BitField<T> lhs, BitField<T> rhs)
        {
            return (lhs._bits & rhs._bits) == rhs._bits;
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is BitField<T> other && _bits == other._bits;
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            return _bits.GetHashCode();
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            return _bits.ToString();
        }
    }
}