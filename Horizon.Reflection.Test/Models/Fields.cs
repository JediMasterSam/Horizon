using System.Reflection;

namespace Horizon.Reflection.Test.Models
{
    public class Fields
    {
        public FieldInfo Public { get; }

        public FieldInfo Internal { get; }

        public FieldInfo Protected { get; }

        public FieldInfo ProtectedInternal { get; }

        public FieldInfo Private { get; }

        public FieldInfo Static { get; }

        public FieldInfo Instance { get; }

        public Fields()
        {
            PublicField = InternalField = ProtectedField = ProtectedInternalField = PrivateField = 0;
            
            Public = typeof(Fields).GetField(nameof(PublicField), BindingFlags.Instance | BindingFlags.Public);
            Internal = typeof(Fields).GetField(nameof(InternalField), BindingFlags.Instance | BindingFlags.NonPublic);
            Protected = typeof(Fields).GetField(nameof(ProtectedField), BindingFlags.Instance | BindingFlags.NonPublic);
            ProtectedInternal = typeof(Fields).GetField(nameof(ProtectedInternalField), BindingFlags.Instance | BindingFlags.NonPublic);
            Private = typeof(Fields).GetField(nameof(PrivateField), BindingFlags.Instance | BindingFlags.NonPublic);
            Static = typeof(Fields).GetField(nameof(StaticField), BindingFlags.Static | BindingFlags.Public);
            Instance = typeof(Fields).GetField(nameof(PublicField), BindingFlags.Instance | BindingFlags.Public);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedReadonlyField
        public readonly int PublicField;

        // ReSharper disable once MemberCanBePrivate.Global
        internal readonly int InternalField;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedReadonlyField
        protected readonly int ProtectedField;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedReadonlyField
        protected internal readonly int ProtectedInternalField;

        private readonly int PrivateField;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedReadonlyField
        public static readonly int StaticField;
    }
}