using Horizon.Numerics;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a modified <see cref="MemberData"/>.
    /// </summary>
    public abstract class ModifierData : MemberData
    {
        /// <summary>
        /// The <see cref="ModifierFlags"/> for the current <see cref="ModifierData"/>.
        /// </summary>
        private readonly BitField<ModifierFlags> _modifierFlags;

        /// <summary>
        /// Creates a new instance of <see cref="ModifierData"/>.
        /// </summary>
        /// <param name="modifierFlags">Modifier flags.</param>
        /// <param name="name">Name.</param>
        /// <param name="path">Path.</param>
        protected internal ModifierData(BitField<ModifierFlags> modifierFlags, string name, string path) : base(name, path)
        {
            _modifierFlags = modifierFlags;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ModifierData"/>.
        /// </summary>
        /// <param name="modifierFlags">Modifier flags.</param>
        /// <param name="name">Name.</param>
        /// <param name="declaringMember">Declaring member.</param>
        protected internal ModifierData(BitField<ModifierFlags> modifierFlags, string name, MemberData declaringMember) : base(name, declaringMember)
        {
            _modifierFlags = modifierFlags;
        }

        /// <summary>
        /// Does the left hand side <see cref="ModifierData"/> contain any of the set bits from the specified right hand side <see cref="ModifierFlags"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="ModifierData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="ModifierFlags"/>.</param>
        /// <returns>True if the specified left hand side <see cref="ModifierData"/> contains any of the set bits in the specified right hand side <see cref="ModifierFlags"/>; otherwise, false.</returns>
        public static bool operator |(ModifierData lhs, ModifierFlags rhs)
        {
            return lhs != null && lhs._modifierFlags | rhs;
        }

        /// <summary>
        /// Does the left hand side <see cref="ModifierData"/> contain all of the set bits from the specified right hand side <see cref="ModifierFlags"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="ModifierData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="ModifierFlags"/>.</param>
        /// <returns>True if the specified left hand side <see cref="ModifierData"/> contains all of the set bits in the specified right hand side <see cref="ModifierFlags"/>; otherwise, false.</returns>
        public static bool operator &(ModifierData lhs, ModifierFlags rhs)
        {
            return lhs != null && lhs._modifierFlags & rhs;
        }
    }
}