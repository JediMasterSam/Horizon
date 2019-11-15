using System.Collections.Generic;
using System.Linq;
using Horizon.OData.Attributes;
using Horizon.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Horizon.OData.Factories
{
    internal static class HeaderFactory
    {
        internal static IReadOnlyList<RequestParameterData> GetAssemblyHeaders(AssemblyData assembly)
        {
            return GetMemberHeaders(assembly).ToArray();
        }

        internal static IReadOnlyList<RequestParameterData> GetControllerHeaders(ControllerData controller)
        {
            var apiVersionHeader = new RequestParameterData("api-version", controller.ApiVersion.ToString(), true, BindingSource.Header, null);
            return JoinHeaders(controller.ApiData.Headers, GetMemberHeaders(controller.ControllerType), new[] {apiVersionHeader}).ToArray();
        }

        internal static IReadOnlyList<RequestParameterData> GetEndpointHeaders(EndpointData endpoint)
        {
            return JoinHeaders(endpoint.Controller.Headers, GetMemberHeaders(endpoint.Method), GetParameterHeaders(endpoint.Method.Parameters)).ToArray();
        }

        private static IEnumerable<RequestParameterData> GetMemberHeaders<TMemberData>(TMemberData memberData) where TMemberData : MemberData
        {
            return memberData.GetAttributes<HeaderAttribute>().Select(headerAttribute => new RequestParameterData(headerAttribute.Name, headerAttribute.DefaultValue, headerAttribute.Required, BindingSource.Header, null));
        }

        private static IEnumerable<RequestParameterData> JoinHeaders(params IEnumerable<RequestParameterData>[] headerSets)
        {
            var headers = new Dictionary<string, RequestParameterData>();

            foreach (var headerSet in headerSets)
            {
                foreach (var header in headerSet)
                {
                    headers[header.Name] = header;
                }
            }

            return headers.Values;
        }

        private static IEnumerable<RequestParameterData> GetParameterHeaders(IEnumerable<ParameterData> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (!parameter.TryGetAttribute<IBindingSourceMetadata>(out var bindingSourceMetadata) || bindingSourceMetadata.BindingSource != BindingSource.Header) continue;

                if (parameter.ParameterType != typeof(string))
                {
                    throw new EndpointException($"Type of {parameter.ParameterType.Path} is not a valid header type. {parameter.Path} should be of type string.", parameter.DeclaringMethod);
                }

                var name = parameter.TryGetAttribute<IModelNameProvider>(out var modelNameProvider) && !string.IsNullOrEmpty(modelNameProvider.Name) ? modelNameProvider.Name : parameter.Name;
                var defaultValue = parameter.IsOptional ? parameter.DefaultValue : null;

                yield return new RequestParameterData(name, defaultValue, defaultValue != null, BindingSource.Header, parameter);
            }
        }
    }
}