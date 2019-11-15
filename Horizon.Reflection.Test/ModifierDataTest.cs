using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class ModifierDataTest : BaseTest
    {
        [TestMethod]
        public void TypeModifierTest()
        {
            var types = new Types();

            var case1 = new TestCase
            {
                ModifierData = types.Public.GetTypeData(),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case2 = new TestCase
            {
                ModifierData = types.PublicNested.GetTypeData(),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case3 = new TestCase
            {
                ModifierData = types.Internal.GetTypeData(),
                ModifierFlags = ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case4 = new TestCase
            {
                ModifierData = types.InternalNested.GetTypeData(),
                ModifierFlags = ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case5 = new TestCase
            {
                ModifierData = types.ProtectedNested.GetTypeData(),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Instance
            };

            var case6 = new TestCase
            {
                ModifierData = types.ProtectedInternalNested.GetTypeData(),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case7 = new TestCase
            {
                ModifierData = types.PrivateNested.GetTypeData(),
                ModifierFlags = ModifierFlags.Private | ModifierFlags.Instance
            };

            var case8 = new TestCase
            {
                ModifierData = types.Abstract.GetTypeData(),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Abstract
            };

            var case9 = new TestCase
            {
                ModifierData = types.Static.GetTypeData(),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Static
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7, case8, case9);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.ModifierData.Modifier.Flags & testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void MethodModifierTest()
        {
            var methods = new Methods();
            var methodsType = typeof(Methods).GetTypeData();
            var baseMethodsType = typeof(MethodsBase).GetTypeData();

            var case1 = new TestCase
            {
                ModifierData = methodsType.Methods.Get(new Name(methods.Public)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case2 = new TestCase
            {
                ModifierData = methodsType.Methods.Get(new Name(methods.Internal)),
                ModifierFlags = ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case3 = new TestCase
            {
                ModifierData = methodsType.Methods.Get(new Name(methods.Protected)),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Instance
            };

            var case4 = new TestCase
            {
                ModifierData = methodsType.Methods.Get(new Name(methods.ProtectedInternal)),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case5 = new TestCase
            {
                ModifierData = baseMethodsType.Methods.Get(new Name(methods.Private)),
                ModifierFlags = ModifierFlags.Private | ModifierFlags.Instance
            };

            var case6 = new TestCase
            {
                ModifierData = baseMethodsType.Methods.Get(new Name(methods.Abstract)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Abstract
            };
            
            var case7 = new TestCase
            {
                ModifierData = methodsType.Methods.Get(new Name(methods.Abstract)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case8 = new TestCase
            {
                ModifierData = methodsType.Methods.Get(new Name(methods.Static)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Static
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7, case8);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.ModifierData.Modifier.Flags & testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void FieldModifierTest()
        {
            var fields = new Fields();
            var fieldsType = typeof(Fields).GetTypeData();

            var case1 = new TestCase
            {
                ModifierData = fieldsType.Fields.Get(new Name(fields.Public)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case2 = new TestCase
            {
                ModifierData = fieldsType.Fields.Get(new Name(fields.Internal)),
                ModifierFlags = ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case3 = new TestCase
            {
                ModifierData = fieldsType.Fields.Get(new Name(fields.Protected)),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Instance
            };

            var case4 = new TestCase
            {
                ModifierData = fieldsType.Fields.Get(new Name(fields.ProtectedInternal)),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case5 = new TestCase
            {
                ModifierData = fieldsType.Fields.Get(new Name(fields.Private)),
                ModifierFlags = ModifierFlags.Private | ModifierFlags.Instance
            };

            var case6 = new TestCase
            {
                ModifierData = fieldsType.Fields.Get(new Name(fields.Static)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Static
            };

            Run(Test, case1, case2, case3, case4, case5, case6);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.ModifierData.Modifier.Flags & testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void PropertyModiferTest()
        {
            var properties = new Properties();
            var propertiesType = typeof(Properties).GetTypeData();
            
            var case1 = new TestCase
            {
                ModifierData = propertiesType.Properties.Get(new Name(properties.Get)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case2 = new TestCase
            {
                ModifierData = propertiesType.Properties.Get(new Name(properties.GetSet)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case3 = new TestCase
            {
                ModifierData = propertiesType.Properties.Get(new Name(properties.GetPrivateSet)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Private | ModifierFlags.Instance
            };

            var case4 = new TestCase
            {
                ModifierData = propertiesType.Properties.Get(new Name(properties.PrivateGetSet)),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Private | ModifierFlags.Instance
            };

            Run(Test, case1, case2, case3, case4);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.ModifierData.Modifier.Flags & testCase.ModifierFlags);
            }
        }

        private class TestCase
        {
            public ModifierData ModifierData { get; set; }

            public ModifierFlags ModifierFlags { get; set; }
        }
    }
}