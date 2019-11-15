using System;

namespace Horizon.OData.Attributes
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