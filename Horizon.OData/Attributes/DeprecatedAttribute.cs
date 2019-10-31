using System;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public sealed class DeprecatedAttribute : Attribute
    {
        public bool Hide { get; set; }
    }
}