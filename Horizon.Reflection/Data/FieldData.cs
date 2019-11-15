using System;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class FieldData : ModifierData
    {
        private readonly FieldInfo _fieldInfo;

        private readonly Lazy<TypeData> _fieldType;

        internal FieldData(FieldInfo fieldInfo, TypeData declaringType) : base(fieldInfo)
        {
            _fieldInfo = fieldInfo;
            _fieldType = new Lazy<TypeData>(() => _fieldInfo.FieldType.GetTypeData());

            DeclaringType = declaringType;
            IsReadOnly = fieldInfo.IsInitOnly;
        }
        
        public TypeData FieldType => _fieldType.Value;
        
        public override TypeData DeclaringType { get; }

        public bool IsReadOnly { get; }

        public static implicit operator FieldInfo(FieldData fieldData)
        {
            return fieldData._fieldInfo;
        }

        public TValue GetValue<TValue>(object obj)
        {
            return (TValue) _fieldInfo.GetValue(obj);
        }

        public void SetValue(object obj, object value)
        {
            _fieldInfo.SetValue(obj, value);
        }
    }
}