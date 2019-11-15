using System;
using System.Collections.Generic;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class FieldDataList : BaseDataList<FieldInfo, FieldData>
    {
        public FieldDataList(TypeData declaringType) : base(declaringType)
        {
        }

        protected override IEnumerable<FieldInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetFields(bindingFlags);
        }

        protected override IEnumerable<FieldData> GetMembers(TypeData typeData)
        {
            return typeData.Fields;
        }

        protected override FieldData Constructor(FieldInfo fieldInfo, TypeData declaringType)
        {
            return new FieldData(fieldInfo, declaringType);
        }
    }
}