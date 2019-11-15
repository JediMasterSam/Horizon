using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class FieldDataTest : BaseTest
    {
        [TestMethod]
        public void FieldDeclarationTest()
        {
            var fields = new Fields();
            var fieldsType = typeof(Fields).GetTypeData();

            var case1 = new TestCase
            {
                FieldData = fieldsType.Fields.Get(new Name(fields.Public)),
                ReadOnly = false,
                DeclaringTypeName = new Name(typeof(Fields))
            };

            var case2 = new TestCase
            {
                FieldData = fieldsType.Fields.Get(new Name(fields.PublicReadOnly)),
                ReadOnly = true,
                DeclaringTypeName = new Name(typeof(BaseFields))
            };

            Run(Test, case1, case2);

            void Test(TestCase testCase)
            {
                AreEqual(testCase.ReadOnly, testCase.FieldData.IsReadOnly);
                AreEqual(testCase.DeclaringTypeName, testCase.FieldData.DeclaringType.Name);
                AreEqual(new Name(typeof(int)), testCase.FieldData.FieldType.Name);
            }
        }

        [TestMethod]
        public void FieldGetTest()
        {
            Run(Test);

            void Test()
            {
                var fields = new Fields {PublicField = 1};
                var fieldData = typeof(Fields).GetTypeData().Fields.Get(new Name(fields.Public));

                AreEqual(1, fieldData.GetValue<int>(fields));
            }
        }

        [TestMethod]
        public void FieldSetTest()
        {
            Run(Test);

            void Test()
            {
                var fields = new Fields();
                var fieldData = typeof(Fields).GetTypeData().Fields.Get(new Name(fields.Public));

                fieldData.SetValue(fields, 1);
                AreEqual(1, fieldData.GetValue<int>(fields));
            }
        }

        private class TestCase
        {
            public FieldData FieldData { get; set; }

            public bool ReadOnly { get; set; }

            public Name DeclaringTypeName { get; set; }
        }
    }
}