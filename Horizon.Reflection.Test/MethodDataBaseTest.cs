using System.Linq;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class MethodDataBaseTest : Diagnostics.BaseTest
    {
        [TestMethod]
        public void GetMethodsTest()
        {
            var case1 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                MethodName = "PublicMethod",
                ModifierFlags = ModifierFlags.Public
            };

            var case2 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                MethodName = "InternalMethod",
                ModifierFlags = ModifierFlags.Internal
            };

            var case3 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                MethodName = "ProtectedMethod",
                ModifierFlags = ModifierFlags.Protected
            };

            var case4 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                MethodName = "PrivateMethod",
                ModifierFlags = ModifierFlags.Private
            };

            var case5 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                MethodName = "PublicMethod",
                ModifierFlags = ModifierFlags.Public
            };

            var case6 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                MethodName = "InternalMethod",
                ModifierFlags = ModifierFlags.Internal
            };

            var case7 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                MethodName = "ProtectedMethod",
                ModifierFlags = ModifierFlags.Protected
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7);

            void Test(GetTestCase testCase)
            {
                var methodData = testCase.TypeData.Methods.FirstOrDefault(method => method.Name == testCase.MethodName);

                if (IsNotNull(methodData))
                {
                    IsTrue(methodData & testCase.ModifierFlags);
                }
            }
        }

        [TestMethod]
        public void CountMethodsTest()
        {
            var case1 = new CountTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                MethodCount = 4
            };

            var case2 = new CountTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                MethodCount = 3
            };

            Run(Test, case1, case2);

            void Test(CountTestCase testCase)
            {
                var objectType = typeof(object).GetTypeData();

                AreEqual(testCase.MethodCount, testCase.TypeData.Methods.Count(method => method.DeclaringType != objectType));
            }
        }

        private class GetTestCase
        {
            public TypeData TypeData { get; set; }

            public string MethodName { get; set; }

            public ModifierFlags ModifierFlags { get; set; }
        }

        private class CountTestCase
        {
            public TypeData TypeData { get; set; }

            public int MethodCount { get; set; }
        }
    }
}