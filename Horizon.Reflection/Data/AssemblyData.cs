using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="Assembly"/>.
    /// </summary>
    public class AssemblyData : MemberData
    {
        /// <summary>
        /// Cached <see cref="Assembly"/>.
        /// </summary>
        private readonly Assembly _assembly;

        /// <summary>
        /// Collection of every <see cref="Type"/> in the current <see cref="AssemblyData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<TypeData>> _types;

        /// <summary>
        /// Creates a new instance of <see cref="AssemblyData"/>.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        internal AssemblyData(Assembly assembly) : base(assembly.GetName().Name, assembly.FullName)
        {
            _assembly = assembly;
            _types = new Lazy<IReadOnlyList<TypeData>>(() => _assembly.GetTypes().Select(type => type.GetTypeData()).ToArray());
        }

        /// <inheritdoc cref="_types"/>
        public IReadOnlyList<TypeData> Types => _types.Value;

        /// <summary>
        /// Implicitly converts the specified <see cref="AssemblyData"/> to <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assemblyData">Assembly data.</param>
        /// <returns>Cached <see cref="Assembly"/>.</returns>
        public static implicit operator Assembly(AssemblyData assemblyData)
        {
            return assemblyData._assembly;
        }
    }
}