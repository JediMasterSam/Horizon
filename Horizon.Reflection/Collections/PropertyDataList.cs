using System;
using System.Collections.Generic;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class PropertyDataList : BaseDataList<PropertyInfo, PropertyData>
    {
        public PropertyDataList(TypeData declaringType) : base(declaringType)
        {
        }

        protected override IEnumerable<PropertyInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetProperties(bindingFlags);
        }

        protected override IEnumerable<PropertyData> GetMembers(TypeData typeData)
        {
            return typeData.Properties;
        }

        protected override PropertyData Constructor(PropertyInfo propertyInfo, TypeData declaringType)
        {
           return new PropertyData(propertyInfo, declaringType);
        }
    }
}