using System.Diagnostics;
using System.Linq;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class PropertyDataBaseTest : Diagnostics.BaseTest
    {
        [TestMethod]
        public void GetPropertiesTest()
        {
            var case1 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                PropertyName = "PublicProperty",
                ModifierFlags = ModifierFlags.Public
            };

            var case2 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                PropertyName = "InternalProperty",
                ModifierFlags = ModifierFlags.Internal
            };

            var case3 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                PropertyName = "ProtectedProperty",
                ModifierFlags = ModifierFlags.Protected
            };

            var case4 = new GetTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                PropertyName = "PrivateProperty",
                ModifierFlags = ModifierFlags.Private
            };

            var case5 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                PropertyName = "PublicProperty",
                ModifierFlags = ModifierFlags.Public
            };

            var case6 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                PropertyName = "InternalProperty",
                ModifierFlags = ModifierFlags.Internal
            };

            var case7 = new GetTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                PropertyName = "ProtectedProperty",
                ModifierFlags = ModifierFlags.Protected
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7);

            void Test(GetTestCase testCase)
            {
                var propertyData = testCase.TypeData.Properties.FirstOrDefault(property => property.Name == testCase.PropertyName);

                if (!IsNotNull(propertyData)) return;
                
                Debug.Assert(propertyData != null, nameof(propertyData) + " != null");
                    
                IsTrue(propertyData.Get & testCase.ModifierFlags);
                IsTrue(propertyData.Set & testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void CountPropertiesTest()
        {
            var case1 = new CountTestCase
            {
                TypeData = typeof(BaseType).GetTypeData(),
                PropertyCount = 4
            };

            var case2 = new CountTestCase
            {
                TypeData = typeof(ChildType).GetTypeData(),
                PropertyCount = 3
            };
            
            Run(Test, case1, case2);

            void Test(CountTestCase testCase)
            {
                AreEqual(testCase.PropertyCount, testCase.TypeData.Properties.Count);
            }
        }

        private class GetTestCase
        {
            public TypeData TypeData { get; set; }

            public string PropertyName { get; set; }

            public ModifierFlags ModifierFlags { get; set; }
        }

        private class CountTestCase
        {
            public TypeData TypeData { get; set; }

            public int PropertyCount { get; set; }
        }
    }
}