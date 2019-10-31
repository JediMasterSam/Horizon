using System;
using Horizon.Reflection;

namespace Horizon.OData
{
    public sealed class ControllerException : Exception
    {
        public ControllerException(string message, MemberData controllerType) : base($"An error occurred while creating the controller {controllerType.Path}: {message}")
        {
        }
    }
}