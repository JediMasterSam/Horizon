using System;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class ConstructorData : MethodBaseData
    {
        private readonly ConstructorInfo _constructorInfo;

        private readonly Lazy<bool> _isDefault;

        internal ConstructorData(ConstructorInfo constructorInfo, TypeData declaringType) : base(constructorInfo, declaringType)
        {
            _constructorInfo = constructorInfo;
            _isDefault = new Lazy<bool>(() => Parameters.Count == 0);
        }

        public bool IsDefault => _isDefault.Value;

        public static implicit operator ConstructorInfo(ConstructorData constructorData)
        {
            return constructorData._constructorInfo;
        }
        
        public TValue Invoke<TValue>(params object[] parameters)
        {
            return (TValue) _constructorInfo.Invoke(parameters);
        }
        
        internal bool TryInvoke<TValue>(object[] parameters, out TValue value)
        {
            if (Parameters.Count != parameters.Length)
            {
                value = default;
                return false;
            }
            
            try
            {
                value = Invoke<TValue>(parameters);
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }
    }
}