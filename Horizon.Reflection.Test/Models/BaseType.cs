namespace Horizon.Reflection.Test.Models
{
    public class BaseType
    {
        public int PublicField;

        internal int InternalField;

        protected int ProtectedField;

        private int PrivateField;

        public int PublicProperty { get; set; }

        internal int InternalProperty { get; set; }

        protected int ProtectedProperty { get; set; }

        private int PrivateProperty { get; set; }

        public void PublicMethod()
        {
        }

        internal void InternalMethod()
        {
        }

        protected void ProtectedMethod()
        {
        }

        private void PrivateMethod()
        {
        }
    }
}