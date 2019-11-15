using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Horizon.OData.Attributes;
using Horizon.Reflection;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Horizon.OData.Factories
{
    internal static class StatusCodeFactory
    {
        private static readonly TypeData ControllerBaseType = typeof(ControllerBase).GetTypeData();

        private static readonly TypeData OdataControllerType = typeof(ODataController).GetTypeData();

        private static readonly TypeData StatusCodeActionResultType = typeof(IStatusCodeActionResult).GetTypeData();

        internal static IEnumerable<HttpStatusCode> GetEndpointStatusCodes(EndpointData endpoint)
        {
            var statusCodes = new HashSet<HttpStatusCode>();

            foreach (var statusCode in GetMemberStatusCodes(endpoint.Method))
            {
                statusCodes.Add(statusCode);
            }

            foreach (var statusCode in GetStatusCodesFromInstructions(endpoint.Method.Instructions))
            {
                statusCodes.Add(statusCode);
            }

            if (statusCodes.Count == 0)
            {
                throw new EndpointException("No HTTP status codes were found.", endpoint.Method);
            }

            return statusCodes;
        }

        private static IEnumerable<HttpStatusCode> GetMemberStatusCodes<TMemberData>(TMemberData member) where TMemberData : MemberData
        {
            return member.GetAttributes<StatusCodeAttribute>().Select(statusCodeAttribute => statusCodeAttribute.StatusCode);
        }

        private static IEnumerable<HttpStatusCode> GetStatusCodesFromInstructions(IEnumerable<Instruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (!(instruction.Operand is MethodBase methodBase)) continue;

                var type = methodBase.DeclaringType.GetTypeData();

                if (type != ControllerBaseType && type != OdataControllerType) continue;
                if (!type.TryGetMethod(methodBase.Name, methodBase.GetParameters().Select(parameter => parameter.ParameterType.GetTypeData()).ToArray(), out var methodBaseData)) continue;
                if (!(methodBaseData is MethodData methodData)) continue;
                if (!methodData.ReturnType.Implements(StatusCodeActionResultType)) continue;
                if (!methodData.ReturnType.TryGetAttribute<DefaultStatusCodeAttribute>(out var defaultStatusCodeAttribute)) continue;

                yield return (HttpStatusCode) defaultStatusCodeAttribute.StatusCode;
            }
        }
    }
}