using System.Reflection;

namespace Horizon.Reflection.Test.Models
{
    public class Fields : BaseFields
    {
        public FieldInfo Public { get; }

        public FieldInfo PublicReadOnly { get; }

        public FieldInfo Internal { get; }

        public FieldInfo Protected { get; }

        public FieldInfo ProtectedInternal { get; }

        public FieldInfo Private { get; }

        public FieldInfo Static { get; }

        public Fields()
        {
            PublicField = InternalField = ProtectedField = ProtectedInternalField = PrivateField = 0;

            Public = typeof(Fields).GetField(nameof(PublicField), BindingFlags.Instance | BindingFlags.Public);
            PublicReadOnly = typeof(Fields).GetField(nameof(PublicReadOnlyField), BindingFlags.Instance | BindingFlags.Public);
            Internal = typeof(Fields).GetField(nameof(InternalField), BindingFlags.Instance | BindingFlags.NonPublic);
            Protected = typeof(Fields).GetField(nameof(ProtectedField), BindingFlags.Instance | BindingFlags.NonPublic);
            ProtectedInternal = typeof(Fields).GetField(nameof(ProtectedInternalField), BindingFlags.Instance | BindingFlags.NonPublic);
            Private = typeof(Fields).GetField(nameof(PrivateField), BindingFlags.Instance | BindingFlags.NonPublic);
            Static = typeof(Fields).GetField(nameof(StaticField), BindingFlags.Static | BindingFlags.Public);
        }

        /// <summary>
        /// PublicField
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedReadonlyField
        public int PublicField;

        /// <summary>
        /// InternalField
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        internal readonly int InternalField;

        /// <summary>
        /// ProtectedField
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedReadonlyField
        protected readonly int ProtectedField;

        /// <summary>
        /// ProtectedInternalField
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedReadonlyField
        protected internal readonly int ProtectedInternalField;

        /// <summary>
        /// PrivateField
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private readonly int PrivateField;

        /// <summary>
        /// StaticField
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedReadonlyField
        public static readonly int StaticField;
    }

    public class BaseFields
    {
        // ReSharper disable once UnassignedReadonlyField
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly int PublicReadOnlyField;

        public BaseFields()
        {
            PublicReadOnlyField = 0;
        }
    }
}