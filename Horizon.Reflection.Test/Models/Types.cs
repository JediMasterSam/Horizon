using System;

namespace Horizon.Reflection.Test.Models
{
    public class Types
    {
        public Type Public { get; }

        public Type PublicNested { get; }

        public Type Internal { get; }

        public Type InternalNested { get; }

        public Type ProtectedNested { get; }

        public Type ProtectedInternalNested { get; }

        public Type PrivateNested { get; }

        public Type Abstract { get; }

        public Type Instance { get; }

        public Type Static { get; }

        public Types()
        {
            Public = GetType();
            PublicNested = typeof(PublicNestedType);
            Internal = typeof(InternalType);
            InternalNested = typeof(InternalNestedType);
            ProtectedNested = typeof(ProtectedNestedType);
            ProtectedInternalNested = typeof(ProtectedInternalNestedType);
            PrivateNested = typeof(PrivateNestedType);
            Abstract = typeof(Abstract);
            Instance = GetType();
            Static = typeof(Static);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public class PublicNestedType
        {
        }

        // ReSharper disable once MemberCanBePrivate.Global
        internal class InternalNestedType
        {
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected class ProtectedNestedType
        {
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected internal class ProtectedInternalNestedType
        {
        }

        private class PrivateNestedType
        {
        }
    }

    internal class InternalType
    {
    }

    public abstract class Abstract
    {
    }

    public static class Static
    {
    }
}