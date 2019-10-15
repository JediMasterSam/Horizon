using System;
using System.Collections.Generic;
using System.Reflection;

namespace Horizon.Reflection
{
    public static class Reflector
    {
        private static readonly Dictionary<Type, TypeData> Types = new Dictionary<Type, TypeData>();

        private static readonly Dictionary<Assembly, AssemblyData> Assemblies = new Dictionary<Assembly, AssemblyData>();

        public static TypeData GetTypeData(this Type type)
        {
            if (type == null) return null;

            if (!Types.TryGetValue(type, out var typeData))
            {
                Types[type] = typeData = new TypeData(type);
            }

            return typeData;
        }

        public static TypeData GetTypeData<TValue>(this TValue value)
        {
            return value.GetType().GetTypeData();
        }

        public static AssemblyData GetAssemblyData(this Assembly assembly)
        {
            if (assembly == null) return null;

            if (!Assemblies.TryGetValue(assembly, out var assemblyData))
            {
                Assemblies[assembly] = assemblyData = new AssemblyData(assembly);
            }

            return assemblyData;
        }
    }
}