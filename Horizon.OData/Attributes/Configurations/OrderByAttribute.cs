using System;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderByAttribute : Attribute
    {
        public OrderByAttribute(bool enableOrderBy)
        {
            EnableOrderBy = enableOrderBy;
        }
        
        public bool EnableOrderBy { get; }
    }
}