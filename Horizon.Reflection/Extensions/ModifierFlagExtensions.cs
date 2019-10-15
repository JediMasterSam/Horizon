using System;
using System.Reflection;
using Horizon.Numerics;

namespace Horizon.Reflection
{
    internal static class ModifierFlagExtensions
    {
        internal static BitField<ModifierFlags> GetModifierFlags(this Type type)
        {
            ModifierFlags modifierFlags;

            if (type.IsPublic || type.IsNestedPublic)
            {
                modifierFlags = ModifierFlags.Public;
            }
            else if (type.IsNestedPrivate)
            {
                modifierFlags = ModifierFlags.Private;
            }
            else if (type.IsNestedFamORAssem)
            {
                modifierFlags = ModifierFlags.Protected | ModifierFlags.Internal;
            }
            else if (type.IsNestedFamily)
            {
                modifierFlags = ModifierFlags.Protected;
            }
            else
            {
                modifierFlags = ModifierFlags.Internal;
            }

            if (type.IsAbstract)
            {
                modifierFlags |= (type.IsSealed ? ModifierFlags.Static : ModifierFlags.Abstract);
            }
            else
            {
                modifierFlags |= ModifierFlags.Instance;
            }

            return modifierFlags;
        }

        internal static BitField<ModifierFlags> GetModifierFlags(this MethodBase methodBase)
        {
            ModifierFlags modifierFlags;

            if (methodBase.IsPublic)
            {
                modifierFlags = ModifierFlags.Public;
            }
            else if (methodBase.IsFamilyOrAssembly)
            {
                modifierFlags = ModifierFlags.Protected | ModifierFlags.Internal;
            }

            else if (methodBase.IsFamily)
            {
                modifierFlags = ModifierFlags.Protected;
            }

            else if (methodBase.IsAssembly)
            {
                modifierFlags = ModifierFlags.Internal;
            }
            else
            {
                modifierFlags = ModifierFlags.Private;
            }

            if (methodBase.IsAbstract)
            {
                modifierFlags |= ModifierFlags.Abstract;
            }
            else if (methodBase.IsStatic)
            {
                modifierFlags |= ModifierFlags.Static;
            }
            else
            {
                modifierFlags |= ModifierFlags.Instance;
            }

            return modifierFlags;
        }

        internal static BitField<ModifierFlags> GetModifierFlags(this FieldInfo fieldInfo)
        {
            ModifierFlags modifierFlags;

            if (fieldInfo.IsPublic)
            {
                modifierFlags = ModifierFlags.Public;
            }
            else if (fieldInfo.IsPrivate)
            {
                modifierFlags = ModifierFlags.Private;
            }
            else if (fieldInfo.IsFamilyOrAssembly)
            {
                modifierFlags = ModifierFlags.Protected | ModifierFlags.Internal;
            }
            else if (fieldInfo.IsFamily)
            {
                modifierFlags = ModifierFlags.Protected;
            }
            else
            {
                modifierFlags = ModifierFlags.Internal;
            }

            if (fieldInfo.IsStatic)
            {
                modifierFlags |= ModifierFlags.Static;
            }
            else
            {
                modifierFlags |= ModifierFlags.Instance;
            }

            return modifierFlags;
        }
    }
}