using System;
using System.Collections.Generic;
using System.Linq;
using Horizon.OData.Factories;
using Horizon.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.OData
{
    public sealed class ControllerData
    {
        private readonly Lazy<ApiVersion> _apiVersion;

        private readonly Lazy<IReadOnlyList<RequestParameterData>> _headers;

        private readonly Lazy<string> _name;

        private readonly Lazy<IReadOnlyList<EndpointData>> _endpoints;

        internal ControllerData(TypeData controllerType, ApiData apiData, bool deprecated)
        {
            _apiVersion = new Lazy<ApiVersion>(() => ApiVersionFactory.GetControllerApiVersion(this));
            _headers = new Lazy<IReadOnlyList<RequestParameterData>>(() => HeaderFactory.GetControllerHeaders(this));
            _name = new Lazy<string>(() => NameFactory.GetControllerName(this));
            _endpoints = new Lazy<IReadOnlyList<EndpointData>>(() => EndpointFactory.GetControllerEndpoints(this).ToArray());

            ControllerType = controllerType;
            ApiData = apiData;
            Deprecated = deprecated;
        }

        public TypeData ControllerType { get; }

        public ProfileData Profile { get; internal set; }

        public ApiData ApiData { get; }

        public bool Deprecated { get; }

        public ApiVersion ApiVersion => _apiVersion.Value;

        public IReadOnlyList<RequestParameterData> Headers => _headers.Value;

        public string Name => _name.Value;

        public IReadOnlyList<EndpointData> Endpoints => _endpoints.Value;
    }
}