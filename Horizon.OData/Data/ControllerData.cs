using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Horizon.OData.Attributes;
using Horizon.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Horizon.OData
{
    public sealed class ControllerData
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

        private readonly Lazy<ApiVersion> _apiVersion;

        private readonly Lazy<IReadOnlyList<IParameter>> _headers;

        private readonly Lazy<string> _name;

        private readonly Lazy<IReadOnlyList<EndpointData>> _endpoints;

        internal ControllerData(TypeData controllerType, ApiData apiData, bool deprecated)
        {
            _apiVersion = new Lazy<ApiVersion>(() => GetApiVersion(ControllerType));
            _headers = new Lazy<IReadOnlyList<IParameter>>(() => GetHeaders(ControllerType, ApiData).ToArray());
            _name = new Lazy<string>(() => GetName(ControllerType));
            _endpoints = new Lazy<IReadOnlyList<EndpointData>>(() => GetEndpoints(this).ToArray());

            ControllerType = controllerType;
            ApiData = apiData;
            Deprecated = deprecated;
        }

        public TypeData ControllerType { get; }

        public ProfileData Profile { get; internal set; }
        
        public ApiData ApiData { get; }

        public bool Deprecated { get; }

        public ApiVersion ApiVersion => _apiVersion.Value;

        public IReadOnlyList<IParameter> Headers => _headers.Value;

        public string Name => _name.Value;

        public IReadOnlyList<EndpointData> Endpoints => _endpoints.Value;

        private static ApiVersion GetApiVersion(MemberData controllerType)
        {
            if (controllerType.TryGetAttribute<ApiVersionAttribute>(out var value))
            {
                var apiVersion = value.Versions.FirstOrDefault();

                if (apiVersion != null)
                {
                    return apiVersion;
                }
            }

            throw new ControllerException($"Could not find {nameof(ApiVersionAttribute)}.", controllerType);
        }

        private static IEnumerable<IParameter> GetHeaders(MemberData controllerType, ApiData apiData)
        {
            foreach (var header in apiData.Headers)
            {
                yield return header;
            }

            foreach (var header in controllerType.GetAttributes<IHeader>())
            {
                yield return header;
            }
        }

        private static string GetName(MemberData controllerType)
        {
            const string suffix = "Controller";

            if (controllerType.Name.EndsWith(suffix))
            {
                return controllerType.Name.Substring(0, controllerType.Name.Length - suffix.Length);
            }

            throw new ControllerException($"The name '{controllerType.Name}' does have the suffix '{suffix}'.", controllerType);
        }

        private static IEnumerable<EndpointData> GetEndpoints(ControllerData controller)
        {
            foreach (var method in controller.ControllerType.Methods)
            {
                if (method.DeclaringType != controller.ControllerType && method.DeclaringType.HasAttribute<DeprecatedAttribute>()) continue;
                if (method.TryGetAttribute<DeprecatedAttribute>(out var deprecated) && deprecated.Hide) continue;
                if (!method.TryGetAttribute<HttpMethodAttribute>(out var value)) continue;

                var name = value.HttpMethods.FirstOrDefault();

                if (name != null && HttpMethods.TryGetValue(name, out var httpMethod))
                {
                    yield return new EndpointData(method, httpMethod, controller, controller.Deprecated || deprecated != null);
                }
            }
        }
    }
}