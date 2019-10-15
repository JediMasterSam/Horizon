using System;

namespace Horizon.Reflection
{
    [Flags]
    public enum DefinitionFlags
    {
        Class = 1,
        Interface = 2,
        Value = 4,
        Primitive = 8 | Value
    }
}