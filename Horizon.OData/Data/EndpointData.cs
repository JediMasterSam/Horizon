using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Horizon.OData.Factories;
using Horizon.Reflection;

namespace Horizon.OData
{
    public sealed class EndpointData
    {
        private readonly Lazy<IReadOnlyList<RequestParameterData>> _headers;

        private readonly Lazy<IReadOnlyList<RequestParameterData>> _requestParameters;

        private readonly Lazy<IReadOnlyList<HttpStatusCode>> _statusCodes;

        private readonly Lazy<TypeData> _responseType;

        internal EndpointData(MethodData method, HttpMethod httpMethod, ControllerData controller, bool deprecated)
        {
            _headers = new Lazy<IReadOnlyList<RequestParameterData>>(() => HeaderFactory.GetEndpointHeaders(this));
            _requestParameters = new Lazy<IReadOnlyList<RequestParameterData>>(() => RequestParameterFactory.GetEndpointParameters(this).ToArray());
            _statusCodes = new Lazy<IReadOnlyList<HttpStatusCode>>(() => StatusCodeFactory.GetEndpointStatusCodes(this).ToArray());
            _responseType = new Lazy<TypeData>(() => ResponseFactory.GetEndpointResponse(this));

            Method = method;
            HttpMethod = httpMethod;
            Controller = controller;
            Deprecated = deprecated;
        }

        public MethodData Method { get; }

        public HttpMethod HttpMethod { get; }

        public ControllerData Controller { get; }

        public bool Deprecated { get; }

        public IReadOnlyList<RequestParameterData> Headers => _headers.Value;

        public IReadOnlyList<RequestParameterData> RequestParameters => _requestParameters.Value;

        public IReadOnlyList<HttpStatusCode> StatusCodes => _statusCodes.Value;

        public TypeData ResponseType => _responseType.Value;
    }
}