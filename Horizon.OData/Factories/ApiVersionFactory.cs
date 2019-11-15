using System.Linq;
using Horizon.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.OData.Factories
{
    internal static class ApiVersionFactory
    {
        internal static ApiVersion GetControllerApiVersion(ControllerData controller)
        {
            var apiVersion = GetMemberApiVersion(controller.ControllerType);

            if (apiVersion == null)
            {
                throw new ControllerException($"Could not find {nameof(ApiVersionAttribute)}.", controller.ControllerType);
            }

            return apiVersion;
        }
        
        private static ApiVersion GetMemberApiVersion<TMemberData>(TMemberData member) where TMemberData : MemberData
        {
            if (!member.TryGetAttribute<ApiVersionAttribute>(out var apiVersionAttribute)) return null;
          
            var apiVersion = apiVersionAttribute.Versions.FirstOrDefault();

            return apiVersion != null ? apiVersion : null;
        }
    }
}