using System.Collections.Generic;
using Horizon.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Horizon.OData.Factories
{
    internal static class RequestParameterFactory
    {
        private static readonly TypeData RequestType = typeof(IRequest).GetTypeData();

        internal static IEnumerable<RequestParameterData> GetEndpointParameters(EndpointData endpoint)
        {
            foreach (var parameter in endpoint.Method.Parameters)
            {
                if (!parameter.TryGetAttribute<IBindingSourceMetadata>(out var bindingSourceMetadata) || bindingSourceMetadata.BindingSource == BindingSource.Header) continue;

                var name = parameter.TryGetAttribute<IModelNameProvider>(out var modelNameProvider) && !string.IsNullOrEmpty(modelNameProvider.Name) ? modelNameProvider.Name : parameter.Name;
                var defaultValue = parameter.ParameterType.Implements(RequestType) && parameter.ParameterType.TryCreate<IRequest>(null, out var response)
                    ? response.GetDefaultValue()
                    : parameter.IsOptional
                        ? parameter.DefaultValue
                        : null;

                yield return new RequestParameterData(name, defaultValue, defaultValue != null, bindingSourceMetadata.BindingSource, parameter);
            }
        }
    }
}