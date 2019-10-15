using System;
using Horizon.Numerics;

namespace Horizon.Reflection
{
    internal static class DefinitionFlagsExtensions
    {
        internal static BitField<DefinitionFlags> GetDefinitionFlags(this Type type)
        {
            if (type.IsClass)
            {
                return DefinitionFlags.Class;
            }

            if (type.IsInterface)
            {
                return DefinitionFlags.Interface;
            }

            return type.IsPrimitive ? DefinitionFlags.Primitive : DefinitionFlags.Value;
        }
    }
}