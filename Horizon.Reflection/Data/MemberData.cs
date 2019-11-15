using System;
using System.Reflection;
using Horizon.Collections;

namespace Horizon.Reflection
{
    public abstract class MemberData
    {
        protected MemberData(Type type)
        {
            Attributes = new ReadOnlyValueList(Attribute.GetCustomAttributes(type, true));
            Name = new Name(type);
        }

        protected MemberData(MemberInfo memberInfo)
        {
            Attributes = new ReadOnlyValueList(Attribute.GetCustomAttributes(memberInfo, true));
            Name = new Name(memberInfo);
        }

        protected MemberData(MethodBase methodBase)
        {
            Attributes = new ReadOnlyValueList(Attribute.GetCustomAttributes(methodBase, true));
            Name = new Name(methodBase);
        }

        protected MemberData(ParameterInfo parameterInfo)
        {
            Attributes = new ReadOnlyValueList(Attribute.GetCustomAttributes(parameterInfo, true));
            Name = new Name(parameterInfo);
        }

        protected MemberData(Assembly assembly)
        {
            Attributes = new ReadOnlyValueList(Attribute.GetCustomAttributes(assembly, true));
            Name = new Name(assembly);
        }

        public ReadOnlyValueList Attributes { get; }

        public Name Name { get; }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && obj is MemberData memberData && Name.Path == memberData.Name.Path;
        }

        public bool Equals(MemberData memberData)
        {
            return !ReferenceEquals(null, memberData) && Name.Path == memberData.Name.Path;
        }

        public override int GetHashCode()
        {
            return Name.Path.GetHashCode();
        }

        public override string ToString()
        {
            return Name.Path;
        }
    }
}