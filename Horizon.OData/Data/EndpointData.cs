using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Horizon.Reflection;

namespace Horizon.OData
{
    public sealed class EndpointData
    {
        private readonly Lazy<IReadOnlyList<IParameter>> _headers;

        internal EndpointData(MethodData method, HttpMethod httpMethod, ControllerData controller, bool deprecated)
        {
            _headers = new Lazy<IReadOnlyList<IParameter>>(() => GetHeaders(Method, Controller).ToArray());

            Method = method;
            HttpMethod = httpMethod;
            Controller = controller;
            Deprecated = deprecated;
        }

        public MethodData Method { get; }

        public HttpMethod HttpMethod { get; }

        public ControllerData Controller { get; }

        public bool Deprecated { get; }

        public IReadOnlyList<IParameter> Headers => _headers.Value;

        private static IEnumerable<IParameter> GetHeaders(MethodBaseData method, ControllerData controller)
        {
            foreach (var header in controller.Headers)
            {
                yield return header;
            }

            foreach (var header in method.GetAttributes<IHeader>())
            {
                yield return header;
            }

            foreach (var parameter in method.Parameters)
            {
                if (!parameter.TryGetAttribute<IHeader>(out var header)) continue;

                if (parameter.ParameterType != typeof(string))
                {
                    throw new EndpointException($"Type of {parameter.ParameterType.Path} is not a valid header type. {parameter.Path} should be of type string.", method);
                }

                if (parameter.IsOptional)
                {
                    header.SetRequired(false);
                    header.SetDefaultValue(parameter.DefaultValue.ToString());
                }

                yield return header;
            }
        }
    }
}