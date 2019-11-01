using System;
using System.Net;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class StatusCodeAttribute : Attribute
    {
        internal StatusCodeAttribute(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        
        public HttpStatusCode StatusCode { get; }
    }
}