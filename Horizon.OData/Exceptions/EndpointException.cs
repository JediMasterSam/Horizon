using System;
using Horizon.Reflection;

namespace Horizon.OData
{
    public sealed class EndpointException : Exception
    {
        public EndpointException(string message, MemberData method) : base($"An error occurred while creating the endpoint {method.Path}: {message}")
        {
        }
    }
}