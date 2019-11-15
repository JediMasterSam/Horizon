using System;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class HeaderAttribute : Attribute
    {
        public HeaderAttribute(string name)
        {
            Name = name;
            Required = true;
        }

        public HeaderAttribute(string name, string defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
            Required = !string.IsNullOrEmpty(defaultValue);
        }

        public string Name { get; }

        public string DefaultValue { get; }

        public bool Required { get; }

    }
}