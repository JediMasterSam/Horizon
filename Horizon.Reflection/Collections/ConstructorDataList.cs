using System;
using System.Collections.Generic;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class ConstructorDataList : BaseDataList<ConstructorInfo, ConstructorData>
    {
        public ConstructorDataList(TypeData declaringType) : base(declaringType)
        {
        }

        protected override IEnumerable<ConstructorInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetConstructors(bindingFlags);
        }

        protected override IEnumerable<ConstructorData> GetMembers(TypeData typeData)
        {
            return typeData.Constructors;
        }

        protected override ConstructorData Constructor(ConstructorInfo constructorInfo, TypeData declaringType)
        {
            return new ConstructorData(constructorInfo, declaringType);
        }
    }
}