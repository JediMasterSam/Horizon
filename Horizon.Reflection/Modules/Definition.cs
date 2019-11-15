using System;
using System.Reflection;
using Horizon.Numerics;

namespace Horizon.Reflection
{
    public readonly struct Definition
    {
        private readonly string _name;
        
        public Definition(Type type)
        {
            DefinitionFlags flags;

            if (type.IsClass)
            {
                flags = DefinitionFlags.Class;
            }
            else if (type.IsInterface)
            {
                flags = DefinitionFlags.Interface;
            }
            else if (type.IsPrimitive)
            {
                flags = DefinitionFlags.Primitive;
            }
            else if (type.IsEnum)
            {
                flags = DefinitionFlags.Enum;
            }
            else
            {
                flags = DefinitionFlags.Value;
            }

            _name = flags.ToString();
            Flags = flags;
        }

        public BitField<DefinitionFlags> Flags { get; }

        public static implicit operator BitField<DefinitionFlags>(Definition definition)
        {
            return definition.Flags;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}