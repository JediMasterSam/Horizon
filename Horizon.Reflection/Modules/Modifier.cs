using System;
using System.Reflection;
using Horizon.Numerics;

namespace Horizon.Reflection
{
    public readonly struct Modifier
    {
        private readonly string _name;
        
        public Modifier(Type type)
        {
            ModifierFlags flags;

            if (type.IsPublic || type.IsNestedPublic)
            {
                flags = ModifierFlags.Public;
            }
            else if (type.IsNestedPrivate)
            {
                flags = ModifierFlags.Private;
            }
            else if (type.IsNestedFamORAssem)
            {
                flags = ModifierFlags.Protected | ModifierFlags.Internal;
            }
            else if (type.IsNestedFamily)
            {
                flags = ModifierFlags.Protected;
            }
            else
            {
                flags = ModifierFlags.Internal;
            }

            if (type.IsAbstract)
            {
                flags |= (type.IsSealed ? ModifierFlags.Static : ModifierFlags.Abstract);
            }
            else
            {
                flags |= ModifierFlags.Instance;
            }

            _name = flags.ToString();
            Flags = flags;
        }

        public Modifier(MethodBase methodBase)
        {
            var flags = GetFlags(methodBase);
            
            _name = flags.ToString();
            Flags = flags;
        }

        public Modifier(FieldInfo fieldInfo)
        {
            ModifierFlags flags;

            if (fieldInfo.IsPublic)
            {
                flags = ModifierFlags.Public;
            }
            else if (fieldInfo.IsPrivate)
            {
                flags = ModifierFlags.Private;
            }
            else if (fieldInfo.IsFamilyOrAssembly)
            {
                flags = ModifierFlags.Protected | ModifierFlags.Internal;
            }
            else if (fieldInfo.IsFamily)
            {
                flags = ModifierFlags.Protected;
            }
            else
            {
                flags = ModifierFlags.Internal;
            }

            if (fieldInfo.IsStatic)
            {
                flags |= ModifierFlags.Static;
            }
            else
            {
                flags |= ModifierFlags.Instance;
            }
            
            _name = flags.ToString();
            Flags = flags;
        }

        public Modifier(PropertyInfo propertyInfo)
        {
            var get = propertyInfo.CanRead ? GetFlags(propertyInfo.GetMethod) : 0;
            var set = propertyInfo.CanWrite ? GetFlags(propertyInfo.SetMethod) : 0;

            var flags = get | set;

            _name = flags.ToString();
            Flags = flags;
        }
        
        public BitField<ModifierFlags> Flags { get; }

        public static implicit operator BitField<ModifierFlags>(Modifier modifier)
        {
            return modifier.Flags;
        }

        private static ModifierFlags GetFlags(MethodBase methodBase)
        {
            ModifierFlags flags;

            if (methodBase.IsPublic)
            {
                flags = ModifierFlags.Public;
            }
            else if (methodBase.IsFamilyOrAssembly)
            {
                flags = ModifierFlags.Protected | ModifierFlags.Internal;
            }

            else if (methodBase.IsFamily)
            {
                flags = ModifierFlags.Protected;
            }

            else if (methodBase.IsAssembly)
            {
                flags = ModifierFlags.Internal;
            }
            else
            {
                flags = ModifierFlags.Private;
            }

            if (methodBase.IsAbstract)
            {
                flags |= ModifierFlags.Abstract;
            }
            else if (methodBase.IsStatic)
            {
                flags |= ModifierFlags.Static;
            }
            else
            {
                flags |= ModifierFlags.Instance;
            }

            return flags;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}