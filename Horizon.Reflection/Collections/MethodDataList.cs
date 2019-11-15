using System;
using System.Collections.Generic;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class MethodDataList : BaseDataList<MethodInfo, MethodData>
    {
        public MethodDataList(TypeData declaringType) : base(declaringType)
        {
        }

        protected override IEnumerable<MethodInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetMethods(bindingFlags);
        }

        protected override IEnumerable<MethodData> GetMembers(TypeData typeData)
        {
            return typeData.Methods;
        }

        protected override MethodData Constructor(MethodInfo methodInfo, TypeData declaringType)
        {
            return new MethodData(methodInfo, declaringType);
        }
    }
}