using System.Collections.Generic;

namespace Horizon.Reflection
{
    public struct MethodBaseDataQuery
    {
        public string Name { get; set; }

        public IReadOnlyList<TypeData> Parameters { get; set; }
    }
}