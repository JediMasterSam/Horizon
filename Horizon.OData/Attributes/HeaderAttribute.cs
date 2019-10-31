using System;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class HeaderAttribute : Attribute, IHeader
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
            Required = false;
        }

        public string Name { get; }

        public string DefaultValue { get; private set; }

        public bool Required { get; private set; }

        void IHeader.SetRequired(bool required)
        {
            Required = required;
        }

        void IHeader.SetDefaultValue(string defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}