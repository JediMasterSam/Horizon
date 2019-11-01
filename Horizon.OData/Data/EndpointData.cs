using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Horizon.OData.Attributes;
using Horizon.Reflection;

namespace Horizon.OData
{
    public sealed class EndpointData
    {
        private readonly Lazy<IReadOnlyList<IParameter>> _headers;

        private readonly Lazy<IReadOnlyList<HttpStatusCode>> _statusCodes;

        private readonly Lazy<TypeData> _responseType;

        internal EndpointData(MethodData method, HttpMethod httpMethod, ControllerData controller, bool deprecated)
        {
            _headers = new Lazy<IReadOnlyList<IParameter>>(() => GetHeaders(Method, Controller).ToArray());
            _statusCodes = new Lazy<IReadOnlyList<HttpStatusCode>>(() => GetStatusCodes(Method).ToArray());
            _responseType = new Lazy<TypeData>(() => GetResponseType(Method));

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

        public IReadOnlyList<HttpStatusCode> StatusCodes => _statusCodes.Value;

        public TypeData ResponseType => _responseType.Value;

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

        private static IEnumerable<HttpStatusCode> GetStatusCodes(MethodBaseData method)
        {
            var found = false;

            foreach (var instruction in method.Instructions)
            {
                if (!instruction.TryGetMethod(out var calledMethod)) continue;
                if (!calledMethod.TryGetAttribute<StatusCodeAttribute>(out var value)) continue;

                found = true;
                yield return value.StatusCode;
            }

            if (!found)
            {
                throw new EndpointException($"No HTTP status codes were found.", method);
            }
        }

        private static TypeData GetResponseType(MethodBaseData method)
        {
            var response = typeof(IResponse).GetTypeData();
            TypeData responseType = null;

            foreach (var instruction in method.Instructions)
            {
                if (instruction.TryGetMethod(out var calledMethod) && calledMethod is ConstructorData && calledMethod.DeclaringType.Implements(response))
                {
                    if (responseType != null)
                    {
                        throw new EndpointException($"{responseType.Path} and {calledMethod.DeclaringType.Path} both implement {response.Path}.", method);
                    }

                    responseType = calledMethod.DeclaringType;
                }
            }

            return responseType;
        }
    }
}