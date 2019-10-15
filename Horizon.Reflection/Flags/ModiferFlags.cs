using System;

namespace Horizon.Reflection
{
    /// <summary>
    /// Defines the accessibility level and declaration modification.
    /// </summary>
    [Flags]
    public enum ModifierFlags
    {
        /// <summary>
        /// Visible everywhere inside and outside of the declared assembly.
        /// </summary>
        Public = 1,

        /// <summary>
        /// Visible everywhere inside of the declared assembly.
        /// </summary>
        Internal = 2,

        /// <summary>
        /// Visible to related types.
        /// </summary>
        Protected = 4,

        /// <summary>
        /// Visible to only within the declared scope.
        /// </summary>
        Private = 8,

        /// <summary>
        /// The implementation is incomplete.
        /// </summary>
        Abstract = 16,

        /// <summary>
        /// The implementation belongs to an object.
        /// </summary>
        Instance = 32 | Abstract,

        /// <summary>
        /// The implementation belongs to the type. 
        /// </summary>
        Static = 64,

        /// <summary>
        /// Public and internal.
        /// </summary>
        Assembly = Public | Internal,

        /// <summary>
        /// Public and protected.
        /// </summary>
        Family = Public | Protected,

        /// <summary>
        /// Internal, protected and private.
        /// </summary>
        NotPublic = Internal | Protected | Private,

        /// <summary>
        /// Public, internal and protected.
        /// </summary>
        AssemblyAndFamily = Public | Internal | Protected
    }
}