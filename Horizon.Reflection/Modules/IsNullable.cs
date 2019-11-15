using System;

namespace Horizon.Reflection
{
    public readonly struct IsNullable
    {
        public IsNullable(Type type)
        {
            Value = !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        public bool Value { get; }

        public static implicit operator bool(IsNullable isNullable)
        {
            return isNullable.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}