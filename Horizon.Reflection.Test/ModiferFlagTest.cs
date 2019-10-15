using System;
using System.Reflection;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class ModifierFlagBaseTest : Diagnostics.BaseTest
    {
        [TestMethod]
        public void TypeGetModifierFlag()
        {
            var types = new Types();

            var case1 = new TestCase<Type>
            {
                Member = types.Public,
                ModifierFlags = ModifierFlags.Public
            };

            var case2 = new TestCase<Type>
            {
                Member = types.PublicNested,
                ModifierFlags = ModifierFlags.Public
            };

            var case3 = new TestCase<Type>
            {
                Member = types.ProtectedNested,
                ModifierFlags = ModifierFlags.Protected
            };

            var case4 = new TestCase<Type>
            {
                Member = types.ProtectedInternalNested,
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal
            };

            var case5 = new TestCase<Type>
            {
                Member = types.InternalNested,
                ModifierFlags = ModifierFlags.Internal
            };

            var case6 = new TestCase<Type>
            {
                Member = types.PrivateNested,
                ModifierFlags = ModifierFlags.Private
            };

            var case7 = new TestCase<Type>
            {
                Member = types.Internal,
                ModifierFlags = ModifierFlags.Internal
            };

            var case8 = new TestCase<Type>
            {
                Member = types.Abstract,
                ModifierFlags = ModifierFlags.Abstract
            };

            var case9 = new TestCase<Type>
            {
                Member = types.Static,
                ModifierFlags = ModifierFlags.Static
            };

            var case10 = new TestCase<Type>
            {
                Member = types.Instance,
                ModifierFlags = ModifierFlags.Instance
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7, case8, case9, case10);

            void Test(TestCase<Type> testCase)
            {
                IsTrue(testCase.Member.GetModifierFlags() | testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void MethodGetModifierFlag()
        {
            var methods = new Methods();

            var case1 = new TestCase<MethodInfo>
            {
                Member = methods.Public,
                ModifierFlags = ModifierFlags.Public
            };

            var case2 = new TestCase<MethodInfo>
            {
                Member = methods.Internal,
                ModifierFlags = ModifierFlags.Internal
            };

            var case3 = new TestCase<MethodInfo>
            {
                Member = methods.Protected,
                ModifierFlags = ModifierFlags.Protected
            };

            var case4 = new TestCase<MethodInfo>
            {
                Member = methods.ProtectedInternal,
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal
            };

            var case5 = new TestCase<MethodInfo>
            {
                Member = methods.Private,
                ModifierFlags = ModifierFlags.Private
            };

            var case6 = new TestCase<MethodInfo>
            {
                Member = methods.Abstract,
                ModifierFlags = ModifierFlags.Abstract
            };

            var case7 = new TestCase<MethodInfo>
            {
                Member = methods.Static,
                ModifierFlags = ModifierFlags.Static
            };

            var case8 = new TestCase<MethodInfo>
            {
                Member = methods.Instance,
                ModifierFlags = ModifierFlags.Instance
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7, case8);

            void Test(TestCase<MethodInfo> testCase)
            {
                IsTrue(testCase.Member.GetModifierFlags() | testCase.ModifierFlags);
            }
        }

        [TestMethod]
        public void FieldGetModifierFlag()
        {
            var fields = new Fields();

            var case1 = new TestCase<FieldInfo>
            {
                Member = fields.Public,
                ModifierFlags = ModifierFlags.Public
            };

            var case2 = new TestCase<FieldInfo>
            {
                Member = fields.Internal,
                ModifierFlags = ModifierFlags.Instance
            };

            var case3 = new TestCase<FieldInfo>
            {
                Member = fields.Protected,
                ModifierFlags = ModifierFlags.Protected
            };

            var case4 = new TestCase<FieldInfo>
            {
                Member = fields.ProtectedInternal,
                ModifierFlags = ModifierFlags.Protected | ModifierFlags.Internal
            };

            var case5 = new TestCase<FieldInfo>
            {
                Member = fields.Private,
                ModifierFlags = ModifierFlags.Private
            };

            var case6 = new TestCase<FieldInfo>
            {
                Member = fields.Static,
                ModifierFlags = ModifierFlags.Static
            };

            var case7 = new TestCase<FieldInfo>
            {
                Member = fields.Instance,
                ModifierFlags = ModifierFlags.Instance
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7);

            void Test(TestCase<FieldInfo> testCase)
            {
                IsTrue(testCase.Member.GetModifierFlags() | testCase.ModifierFlags);
            }
        }

        private class TestCase<T>
        {
            public T Member { get; set; }

            public ModifierFlags ModifierFlags { get; set; }
        }
    }
}