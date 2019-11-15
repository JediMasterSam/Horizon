using System;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class PropertyData : ModifierData
    {
        private readonly PropertyInfo _propertyInfo;

        private readonly Lazy<TypeData> _propertyType;

        private readonly Lazy<MethodData> _get;

        private readonly Lazy<MethodData> _set;

        internal PropertyData(PropertyInfo propertyInfo, TypeData declaringType) : base(propertyInfo)
        {
            _propertyInfo = propertyInfo;
            _propertyType = new Lazy<TypeData>(() => _propertyInfo.PropertyType.GetTypeData());
            _get = new Lazy<MethodData>(() => _propertyInfo.GetMethod != null ? new MethodData(_propertyInfo.GetMethod, DeclaringType) : null);
            _set = new Lazy<MethodData>(() => _propertyInfo.SetMethod != null ? new MethodData(_propertyInfo.SetMethod, DeclaringType) : null);

            DeclaringType = declaringType;
            CanRead = propertyInfo.CanRead;
            CanWrite = propertyInfo.CanWrite;
        }

        public TypeData PropertyType => _propertyType.Value;

        public MethodData Get => _get.Value;

        public MethodData Set => _set.Value;
        
        public override TypeData DeclaringType { get; }

        public bool CanRead { get; }
        
        public bool CanWrite { get; }

        public static implicit operator PropertyInfo(PropertyData propertyData)
        {
            return propertyData._propertyInfo;
        }
    }
}