using Horizon.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Horizon.OData
{
    public sealed class RequestParameterData
    {
        public RequestParameterData(string name, object defaultValue, bool required, BindingSource bindingSource, ParameterData parameter)
        {
            Name = name;
            DefaultValue = defaultValue;
            Required = required;
            BindingSource = bindingSource;
            Parameter = parameter;
        }
        
        public string Name { get; }
        
        public object DefaultValue { get; }
        
        public bool Required { get; }
        
        public BindingSource BindingSource { get; }
        
        public ParameterData Parameter { get; }
    }
}