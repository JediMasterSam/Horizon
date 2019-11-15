using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class ModifierTest : BaseTest
    {
        [TestMethod]
        public void TypeModifierTest()
        {
            var types = new Types();

            var case1 = new TestCase
            {
                Modifier = new Modifier(types.Public),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case2 = new TestCase
            {
                Modifier = new Modifier(types.PublicNested),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case3 = new TestCase
            {
                Modifier = new Modifier(types.Internal),
                ModifierFlags = ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case4 = new TestCase
            {
                Modifier = new Modifier(types.InternalNested),
                ModifierFlags = ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case5 = new TestCase
            {
                Modifier = new Modifier(types.ProtectedNested),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Instance
            };

            var case6 = new TestCase
            {
                Modifier = new Modifier(types.ProtectedInternalNested),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case7 = new TestCase
            {
                Modifier = new Modifier(types.PrivateNested),
                ModifierFlags = ModifierFlags.Private | ModifierFlags.Instance
            };

            var case8 = new TestCase
            {
                Modifier = new Modifier(types.Abstract),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Abstract
            };

            var case9 = new TestCase
            {
                Modifier = new Modifier(types.Static),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Static
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7, case8, case9);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.Modifier.Flags & testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void MethodModifierTest()
        {
            var methods = new Methods();

            var case1 = new TestCase
            {
                Modifier = new Modifier(methods.Public),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case2 = new TestCase
            {
                Modifier = new Modifier(methods.Internal),
                ModifierFlags = ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case3 = new TestCase
            {
                Modifier = new Modifier(methods.Protected),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Instance
            };

            var case4 = new TestCase
            {
                Modifier = new Modifier(methods.ProtectedInternal),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case5 = new TestCase
            {
                Modifier = new Modifier(methods.Private),
                ModifierFlags = ModifierFlags.Private | ModifierFlags.Instance
            };

            var case6 = new TestCase
            {
                Modifier = new Modifier(methods.Abstract),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Abstract
            };

            var case7 = new TestCase
            {
                Modifier = new Modifier(methods.Static),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Static
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.Modifier.Flags & testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void FieldModifierTest()
        {
            var fields = new Fields();

            var case1 = new TestCase
            {
                Modifier = new Modifier(fields.Public),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case2 = new TestCase
            {
                Modifier = new Modifier(fields.Internal),
                ModifierFlags = ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case3 = new TestCase
            {
                Modifier = new Modifier(fields.Protected),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Instance
            };

            var case4 = new TestCase
            {
                Modifier = new Modifier(fields.ProtectedInternal),
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal | ModifierFlags.Instance
            };

            var case5 = new TestCase
            {
                Modifier = new Modifier(fields.Private),
                ModifierFlags = ModifierFlags.Private | ModifierFlags.Instance
            };

            var case6 = new TestCase
            {
                Modifier = new Modifier(fields.Static),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Static
            };

            Run(Test, case1, case2, case3, case4, case5, case6);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.Modifier.Flags & testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void PropertyModiferTest()
        {
            var properties = new Properties();

            var case1 = new TestCase
            {
                Modifier = new Modifier(properties.Get),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case2 = new TestCase
            {
                Modifier = new Modifier(properties.GetSet),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Instance
            };

            var case3 = new TestCase
            {
                Modifier = new Modifier(properties.GetPrivateSet),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Private | ModifierFlags.Instance
            };

            var case4 = new TestCase
            {
                Modifier = new Modifier(properties.PrivateGetSet),
                ModifierFlags = ModifierFlags.Public | ModifierFlags.Private | ModifierFlags.Instance
            };

            Run(Test, case1, case2, case3, case4);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.Modifier.Flags & testCase.ModifierFlags);
            }
        }

//        [TestMethod]
//        public void DeclaringTypeTest()
//        {
//            var methods = new Methods();
//
//            var case1 = new DeclaringTypeTestCase
//            {
//                MethodBaseData = new MethodBaseData(methods.Public),
//                DeclaringTypeName = new Name(typeof(MethodsBase))
//            };
//
//            var case2 = new DeclaringTypeTestCase
//            {
//                MethodBaseData = new MethodBaseData(methods.Abstract),
//                DeclaringTypeName = new Name(typeof(MethodsBase))
//            };
//
//            var case3 = new DeclaringTypeTestCase
//            {
//                MethodBaseData = new MethodBaseData(methods.Declared),
//                DeclaringTypeName = new Name(typeof(Methods))
//            };
//
//            Run(Test, case1, case2, case3);
//
//            void Test(DeclaringTypeTestCase testCase)
//            {
//                AreEqual(testCase.DeclaringTypeName, testCase.MethodBaseData.DeclaringType.Name);
//            }
//        }

        private class TestCase
        {
            public Modifier Modifier { get; set; }

            public ModifierFlags ModifierFlags { get; set; }
        }

//        private class DeclaringTypeTestCase
//        {
//            public MethodBaseData MethodBaseData { get; set; }
//
//            public Name DeclaringTypeName { get; set; }
//        }
    }
}