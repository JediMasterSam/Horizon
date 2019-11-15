using System;
using Microsoft.AspNet.OData.Query;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectAttribute : Attribute
    {
        public SelectAttribute(SelectExpandType selectExpandType)
        {
            SelectExpandType = selectExpandType;
        }
        
        public SelectExpandType SelectExpandType { get; }
    }
}