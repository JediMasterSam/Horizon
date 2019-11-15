using System.Reflection;

namespace Horizon.Reflection.Test.Models
{
    public class Methods : MethodsBase
    {
        public MethodInfo Declared { get; }

        public Methods()
        {
            Declared = typeof(Methods).GetMethod(nameof(DeclaredMethod), BindingFlags.Instance | BindingFlags.Public);
        }

        public override void AbstractMethod()
        {
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public void DeclaredMethod()
        {
        }
    }

    public abstract class MethodsBase
    {
        public MethodInfo Public { get; }

        public MethodInfo Internal { get; }

        public MethodInfo Protected { get; }

        public MethodInfo ProtectedInternal { get; }

        public MethodInfo Private { get; }

        public MethodInfo Abstract { get; }

        public MethodInfo Static { get; }

        protected MethodsBase()
        {
            Public = typeof(MethodsBase).GetMethod(nameof(PublicMethod), BindingFlags.Instance | BindingFlags.Public);
            Internal = typeof(MethodsBase).GetMethod(nameof(InternalMethod), BindingFlags.Instance | BindingFlags.NonPublic);
            Protected = typeof(MethodsBase).GetMethod(nameof(ProtectedMethod), BindingFlags.Instance | BindingFlags.NonPublic);
            ProtectedInternal = typeof(MethodsBase).GetMethod(nameof(ProtectedInternalMethod), BindingFlags.Instance | BindingFlags.NonPublic);
            Private = typeof(MethodsBase).GetMethod(nameof(PrivateMethod), BindingFlags.Instance | BindingFlags.NonPublic);
            Abstract = typeof(MethodsBase).GetMethod(nameof(AbstractMethod), BindingFlags.Instance | BindingFlags.Public);
            Static = typeof(MethodsBase).GetMethod(nameof(StaticMethod), BindingFlags.Static | BindingFlags.Public);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void PublicMethod()
        {
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        // ReSharper disable once MemberCanBePrivate.Global
        internal void InternalMethod()
        {
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        // ReSharper disable once MemberCanBePrivate.Global
        protected void ProtectedMethod()
        {
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        // ReSharper disable once MemberCanBePrivate.Global
        protected internal void ProtectedInternalMethod()
        {
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private void PrivateMethod()
        {
        }

        // ReSharper disable once MemberCanBeProtected.Global
        public abstract void AbstractMethod();

        // ReSharper disable once MemberCanBePrivate.Global
        public static void StaticMethod()
        {
        }
    }
}