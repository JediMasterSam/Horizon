using System;
using Horizon.OData.Attributes;

namespace Horizon.OData
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntitySetAttribute : EntityTypeAttribute
    {
        public EntitySetAttribute(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}