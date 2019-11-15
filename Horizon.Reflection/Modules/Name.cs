using System;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    public readonly struct Name
    {
        public Name(Type type)
        {
            Value = type.Name;
            Path = type.FullName ?? Value;
        }

        public Name(MemberInfo memberInfo)
        {
            var declaringType = memberInfo.DeclaringType;

            Value = memberInfo.Name;
            Path = declaringType != null ? $"{declaringType.FullName}.{Value}" : Value;
        }

        public Name(MethodBase methodBase)
        {
            var declaringType = methodBase.DeclaringType;
            var parameters = methodBase.GetParameters();
            var parameterSignature = parameters.Length == 0 ? string.Empty : $"({string.Join(",", parameters.Select(parameter => new Name(parameter.ParameterType).Path))})";

            var name = methodBase.Name;

            Value = name[0] == '.' ? $"{name.Substring(1)}{parameterSignature}" : $"{name}{parameterSignature}";
            Path = declaringType != null ? $"{declaringType.FullName}.{Value}" : $"{Value}";
        }

        public Name(ParameterInfo parameterInfo)
        {
            var declaringMember = parameterInfo.Member;
            var declaringType = declaringMember.DeclaringType;
            
            Value = parameterInfo.Name;
            Path = declaringType != null ? $"{declaringType.FullName}.{declaringMember.Name}.{Value}" : $"{declaringMember.Name}.{Value}";
        }

        public Name(Assembly assembly)
        {
            var assemblyName = assembly.GetName();

            Value = assemblyName.Name;
            Path = assemblyName.FullName;
        }

        public string Value { get; }

        public string Path { get; }

        public static implicit operator string(Name name)
        {
            return name.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Name name && Path == name.Path;
        }

        public bool Equals(Name name)
        {
            return Path == name.Path;
        }

        public override int GetHashCode()
        {
            return Path != null ? Path.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}