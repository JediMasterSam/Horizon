using System.Linq;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class FieldDataBaseTest : Diagnostics.BaseTest
    {
        [TestMethod]
        public void GetFieldsTest()
        {
            var case1 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                FieldName = "PublicField",
                ModifierFlags = ModifierFlags.Public
            };

            var case2 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                FieldName = "InternalField",
                ModifierFlags = ModifierFlags.Internal
            };

            var case3 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                FieldName = "ProtectedField",
                ModifierFlags = ModifierFlags.Protected
            };

            var case4 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                FieldName = "PrivateField",
                ModifierFlags = ModifierFlags.Private
            };

            var case5 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                FieldName = "PublicField",
                ModifierFlags = ModifierFlags.Public
            };

            var case6 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                FieldName = "InternalField",
                ModifierFlags = ModifierFlags.Internal
            };

            var case7 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                FieldName = "ProtectedField",
                ModifierFlags = ModifierFlags.Protected
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7);

            void Test(GetTestCase testCase)
            {
                var fieldData = testCase.TypeData.Fields.FirstOrDefault(field => field.Name == testCase.FieldName);

                if (IsNotNull(fieldData))
                {
                    IsTrue(fieldData & testCase.ModifierFlags);
                }
            }
        }

        [TestMethod]
        public void CountFieldsTest()
        {
            var case1 = new CountTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                FieldCount = 4
            };

            var case2 = new CountTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                FieldCount = 3
            };
            
            Run(Test, case1, case2);

            void Test(CountTestCase testCase)
            {
                AreEqual(testCase.FieldCount, testCase.TypeData.Fields.Count);
            }
        }

        private class GetTestCase
        {
            public TypeData TypeData { get; set; }

            public string FieldName { get; set; }

            public ModifierFlags ModifierFlags { get; set; }
        }

        private class CountTestCase
        {
            public TypeData TypeData { get; set; }

            public int FieldCount { get; set; }
        }
    }
}