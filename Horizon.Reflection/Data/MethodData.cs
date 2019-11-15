using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class MethodData : MethodBaseData
    {
        private readonly MethodInfo _methodInfo;

        private readonly Lazy<TypeData> _returnType;

        private readonly Lazy<IReadOnlyList<TypeData>> _genericArguments;
        
        internal MethodData(MethodInfo methodInfo, TypeData declaringType) : base(methodInfo, declaringType)
        {
            _methodInfo = methodInfo;
            _returnType = new Lazy<TypeData>(() => methodInfo.ReturnType.GetTypeData());
            _genericArguments = new Lazy<IReadOnlyList<TypeData>>(() => IsGenericMethod ? _methodInfo.GetGenericArguments().Select(TypeCache.GetTypeData).ToArray() : null);

            IsGenericMethod = methodInfo.IsGenericMethod;
        }

        public TypeData ReturnType => _returnType.Value;

        public IReadOnlyList<TypeData> GenericArguments => _genericArguments.Value;

        public MethodData GenericMethodDefinition { get; private set; }

        public bool IsGenericMethod { get; }

        public static implicit operator MethodInfo(MethodData methodData)
        {
            return methodData._methodInfo;
        }

        public MethodData MakeGenericMethod(params Type[] typeArguments)
        {
            return new MethodData(_methodInfo.MakeGenericMethod(typeArguments), DeclaringType) {GenericMethodDefinition = this};
        }
    }
}