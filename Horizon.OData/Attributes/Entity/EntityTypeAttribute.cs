using System;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityTypeAttribute : Attribute
    {
        public bool EnableCount { get; set; }
        
        public int? MaxTop { get; set; }
        
        public int? PageSize { get; set; }
    }
}