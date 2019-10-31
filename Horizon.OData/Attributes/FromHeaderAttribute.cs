using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Horizon.OData.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromHeaderAttribute : ParameterBindingAttribute, IHeader
    {
        public FromHeaderAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string DefaultValue { get; private set; }

        public bool Required { get; private set; }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new FromHeaderBinding(parameter, this);
        }

        void IHeader.SetRequired(bool required)
        {
            Required = required;
        }

        void IHeader.SetDefaultValue(string defaultValue)
        {
            DefaultValue = defaultValue;
        }

        private class FromHeaderBinding : HttpParameterBinding
        {
            private readonly IHeader _header;

            public FromHeaderBinding(HttpParameterDescriptor descriptor, IHeader header) : base(descriptor)
            {
                _header = header;
            }

            public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
            {
                if (actionContext.Request.Headers.TryGetValues(_header.Name, out var values))
                {
                    actionContext.ActionArguments[Descriptor.ParameterName] = values.FirstOrDefault();
                }

                return Task.CompletedTask;
            }
        }
    }
}