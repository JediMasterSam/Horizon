using System;
using Microsoft.AspNet.OData.Query;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpandAttribute : Attribute
    {
        public ExpandAttribute(int maxDepth, SelectExpandType selectExpandType)
        {
            MaxDepth = maxDepth;
            SelectExpandType = selectExpandType;
        }
        
        public int MaxDepth { get; }
        
        public SelectExpandType SelectExpandType { get; }
    }
}