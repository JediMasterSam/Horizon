using System;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class ParameterData : MemberData
    {
        private readonly ParameterInfo _parameterInfo;

        private readonly Lazy<TypeData> _parameterType;

        internal ParameterData(ParameterInfo parameterInfo, MethodBaseData declaringMethod) : base(parameterInfo)
        {
            _parameterInfo = parameterInfo;
            _parameterType = new Lazy<TypeData>(() => _parameterInfo.ParameterType.GetTypeData());
            
            DeclaringMethod = declaringMethod;
            IsOut = parameterInfo.IsOut;
            IsOptional = parameterInfo.IsOptional;
            DefaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null;
        }

        public TypeData ParameterType => _parameterType.Value;

        public MethodBaseData DeclaringMethod { get; }

        public bool IsOut { get; }

        public bool IsOptional { get; }

        public object DefaultValue { get; }

        public static implicit operator ParameterInfo(ParameterData parameterData)
        {
            return parameterData._parameterInfo;
        }
    }
}