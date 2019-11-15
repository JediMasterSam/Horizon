using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class AssemblyData : MemberData
    {
        private readonly Assembly _assembly;

        private readonly Lazy<IReadOnlyList<TypeData>> _types;
        
        internal AssemblyData(Assembly assembly) : base(assembly)
        {
            _assembly = assembly;
            _types = new Lazy<IReadOnlyList<TypeData>>(() => assembly.GetTypes().Select(type => type.GetTypeData()).ToArray());
        }

        public IReadOnlyList<TypeData> Types => _types.Value;

        public static implicit operator Assembly(AssemblyData assemblyData)
        {
            return assemblyData._assembly;
        }
    }
}