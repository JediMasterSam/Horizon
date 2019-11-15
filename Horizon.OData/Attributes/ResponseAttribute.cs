using System;
using Horizon.Reflection;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ResponseAttribute : Attribute
    {
        public ResponseAttribute(Type responseType)
        {
            ResponseType = responseType.GetTypeData();
        }
        
        public TypeData ResponseType { get; }
    }
}