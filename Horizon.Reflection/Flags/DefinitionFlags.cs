using System;

namespace Horizon.Reflection
{
    /// <summary>
    /// Defines the characterization of a <see cref="Type"/>.
    /// </summary>
    [Flags]
    public enum DefinitionFlags
    {
        /// <summary>
        /// Reference type.
        /// </summary>
        Class = 1,

        /// <summary>
        /// Reference type contract.
        /// </summary>
        Interface = 2,

        /// <summary>
        /// Value type.
        /// </summary>
        Value = 4,

        /// <summary>
        /// Predefined value type.
        /// </summary>
        Primitive = 8 | Value,
        
        /// <summary>
        /// Named constant.
        /// </summary>
        Enum = 16 | Value
    }
}