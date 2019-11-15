using System.Reflection;

namespace Horizon.Reflection.Test.Models
{
    public class Properties
    {
        public PropertyInfo Get { get; }

        public PropertyInfo GetSet { get; }

        public PropertyInfo GetPrivateSet { get; }

        public PropertyInfo PrivateGetSet { get; }

        public Properties()
        {
            Get = GetType().GetProperty(nameof(GetProperty));
            GetSet = GetType().GetProperty(nameof(GetSetProperty));
            GetPrivateSet = GetType().GetProperty(nameof(GetPrivateSetProperty));
            PrivateGetSet = GetType().GetProperty(nameof(PrivateGetSetProperty));
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int GetProperty { get; }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int GetSetProperty { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int GetPrivateSetProperty { get; private set; }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public int PrivateGetSetProperty { private get; set; }
    }
}