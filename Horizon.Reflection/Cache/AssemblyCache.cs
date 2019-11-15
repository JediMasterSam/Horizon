using System.Collections.Generic;
using System.Reflection;

namespace Horizon.Reflection
{
    public static class AssemblyCache
    {
        private static readonly Dictionary<Assembly, AssemblyData> Assemblies = new Dictionary<Assembly, AssemblyData>();

        public static AssemblyData GetAssemblyData(this Assembly assembly)
        {
            if (assembly == null)
            {
                return null;
            }

            if (Assemblies.TryGetValue(assembly, out var assemblyData))
            {
                return assemblyData;
            }

            Assemblies[assembly] = assemblyData = new AssemblyData(assembly);

            return assemblyData;
        }

        public static AssemblyData GetAssemblyData<TValue>(TValue value)
        {
            return value == null ? null : value.GetTypeData().Assembly;
        }
    }
}