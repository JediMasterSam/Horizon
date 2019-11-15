using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Horizon.OData.Attributes;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Horizon.OData.Factories
{
    internal static class EndpointFactory
    {
        private static readonly Dictionary<string, HttpMethod> HttpMethods = new Dictionary<string, HttpMethod>
        {
            {HttpMethod.Delete.Method, HttpMethod.Delete},
            {HttpMethod.Get.Method, HttpMethod.Get},
            {HttpMethod.Head.Method, HttpMethod.Head},
            {HttpMethod.Options.Method, HttpMethod.Options},
            {HttpMethod.Patch.Method, HttpMethod.Patch},
            {HttpMethod.Post.Method, HttpMethod.Post},
            {HttpMethod.Put.Method, HttpMethod.Put},
            {HttpMethod.Trace.Method, HttpMethod.Trace}
        };

        internal static IEnumerable<EndpointData> GetControllerEndpoints(ControllerData controller)
        {
            foreach (var method in controller.ControllerType.Methods)
            {
                if (method.DeclaringType != controller.ControllerType && method.DeclaringType.HasAttribute<DeprecatedAttribute>()) continue;
                if (method.TryGetAttribute<DeprecatedAttribute>(out var deprecatedAttribute) && deprecatedAttribute.Hide) continue;
                if (!method.TryGetAttribute<HttpMethodAttribute>(out var httpMethodAttribute)) continue;

                var name = httpMethodAttribute.HttpMethods.FirstOrDefault();

                if (name != null && HttpMethods.TryGetValue(name, out var httpMethod))
                {
                    yield return new EndpointData(method, httpMethod, controller, controller.Deprecated || deprecatedAttribute != null);
                }
            }
        }
    }
}