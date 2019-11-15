using System.Net.Http;
using Horizon.OData.Attributes;
using Horizon.Reflection;

namespace Horizon.OData.Factories
{
    internal static class ResponseFactory
    {
        internal static TypeData GetEndpointResponse(EndpointData endpoint)
        {
            var response = GetMemberResponse(endpoint.Method);

            if (response == null && endpoint.HttpMethod == HttpMethod.Get)
            {
                throw new EndpointException($"HTTP GET must have a return type; add {nameof(ResponseAttribute)}.", endpoint.Method);
            }
            
            return response;
        }
        
        private static TypeData GetMemberResponse<TMemberData>(TMemberData member) where TMemberData : MemberData
        {
            return member.TryGetAttribute<ResponseAttribute>(out var responseAttribute) ? responseAttribute.ResponseType : null;
        }
    }
}