using System;
using System.Reflection;
using Horizon.Numerics;

namespace Horizon.Reflection
{
    internal static class DefinitionFlagsExtensions
    {
        internal static BitField<DefinitionFlags> GetDefinitionFlags(this Type type)
        {
            DefinitionFlags definitionFlags;

            if (type.IsClass)
            {
                definitionFlags = DefinitionFlags.Class;
            }
            else if (type.IsInterface)
            {
                definitionFlags = DefinitionFlags.Interface;
            }
            else if (type.IsPrimitive)
            {
                definitionFlags = DefinitionFlags.Primitive;
            }
            else
            {
                definitionFlags = DefinitionFlags.Value;
            }

            if (type.IsConstructedGenericType)
            {
                definitionFlags |= DefinitionFlags.ConstructedGeneric;
            }
            else if (type.IsGenericTypeDefinition)
            {
                definitionFlags |= DefinitionFlags.DeconstructedGeneric;
            }

            return definitionFlags;
        }

        internal static DefinitionFlags GetDefinitionFlags(this MethodInfo methodInfo)
        {
            if (!methodInfo.IsGenericMethod)
            {
                return 0;
            }

            return methodInfo.IsConstructedGenericMethod ? DefinitionFlags.ConstructedGeneric : DefinitionFlags.DeconstructedGeneric;
        }
    }
}