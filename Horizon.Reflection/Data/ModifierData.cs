using System;
using System.Reflection;

namespace Horizon.Reflection
{
    public abstract class ModifierData : MemberData
    {
        protected ModifierData(Type type) : base(type)
        {
            Modifier = new Modifier(type);
        }

        protected ModifierData(FieldInfo fieldInfo) : base(fieldInfo)
        {
            Modifier = new Modifier(fieldInfo);
        }

        protected ModifierData(MethodBase methodBase) : base(methodBase)
        {
            Modifier = new Modifier(methodBase);
        }

        protected ModifierData(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            Modifier = new Modifier(propertyInfo);
        }

        public Modifier Modifier { get; }

        public abstract TypeData DeclaringType { get; }
    }
}