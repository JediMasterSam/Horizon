using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    public abstract class MethodBaseData : ModifierData
    {
        private readonly MethodBase _methodBase;

        private readonly Lazy<IReadOnlyList<ParameterData>> _parameters;

        protected MethodBaseData(MethodBase methodBase, TypeData declaringType) : base(methodBase)
        {
            _methodBase = methodBase;
            _parameters = new Lazy<IReadOnlyList<ParameterData>>(() => _methodBase.GetParameters().Select(parameterInfo => new ParameterData(parameterInfo, this)).ToArray());

            DeclaringType = declaringType;
        }

        public IReadOnlyList<ParameterData> Parameters => _parameters.Value;
        
        public override TypeData DeclaringType { get; }

        public static implicit operator MethodBase(MethodBaseData methodBaseData)
        {
            return methodBaseData._methodBase;
        }

        public void Invoke(object obj, params object[] parameters)
        {
            _methodBase.Invoke(obj, parameters);
        }

        public TValue Invoke<TValue>(object obj, params object[] parameters)
        {
            return (TValue) _methodBase.Invoke(obj, parameters);
        }
    }
}