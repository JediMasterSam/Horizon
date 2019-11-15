using System;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterAttribute : Attribute
    {
        public FilterAttribute(bool enableFilter)
        {
            EnableFilter = enableFilter;
        }
        
        public bool EnableFilter { get; }
    }
}