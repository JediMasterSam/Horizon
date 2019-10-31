using System;
using Horizon.Reflection;

namespace Horizon.OData
{
    public class ProfileException : Exception
    {
        public ProfileException(string message, MemberData profileType) : base($"An error occurred while creating the profile {profileType.Path}: {message}")
        {
        }
    }
}