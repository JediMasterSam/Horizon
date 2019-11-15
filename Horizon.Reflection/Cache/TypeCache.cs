using System;
using System.Collections.Generic;

namespace Horizon.Reflection
{
    public static class TypeCache
    {
        private static readonly Dictionary<Type, TypeData> Types = new Dictionary<Type, TypeData>();

        public static TypeData GetTypeData(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (Types.TryGetValue(type, out var typeData))
            {
                return typeData;
            }

            Types[type] = typeData = new TypeData(type);

            return typeData;
        }

        public static TypeData GetTypeData<TValue>(this TValue value)
        {
            return value == null ? null : GetTypeData(typeof(TValue));
        }
    }
}