using System;
using System.Net;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class StatusCodeAttribute : Attribute
    {
        internal StatusCodeAttribute(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        
        public HttpStatusCode StatusCode { get; }
    }
}