using System;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonTypeAttribute : EntityTypeAttribute
    {
        public SingletonTypeAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}