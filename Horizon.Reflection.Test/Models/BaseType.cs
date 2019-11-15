namespace Horizon.Reflection.Test.Models
{
    public class BaseType
    {
        /// <summary>
        /// PublicField
        /// </summary>
        public int PublicField;

        /// <summary>
        /// InternalField
        /// </summary>
        internal int InternalField;

        /// <summary>
        /// ProtectedField
        /// </summary>
        protected int ProtectedField;

        /// <summary>
        /// PrivateField
        /// </summary>
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