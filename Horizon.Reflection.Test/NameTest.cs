using System;
using System.Linq;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class NameTest : BaseTest
    {
        [TestMethod]
        public void TypeNameTest()
        {
            Run(Test);

            void Test()
            {
                var name = new Name(typeof(Names));

                AreEqual(nameof(Names), name);
                AreEqual($"{typeof(Names).FullName}", name.Path);
            }
        }

        [TestMethod]
        public void MemberNameTest()
        {
            var case1 = new TestCase
            {
                Name = new Name(typeof(Names).GetField(nameof(Names.Field))),
                ExpectedName = nameof(Names.Field),
                ExpectedPath = $"{typeof(Names).FullName}.{nameof(Names.Field)}"
            };

            var case2 = new TestCase
            {
                Name = new Name(typeof(Names).GetProperty(nameof(Names.Property))),
                ExpectedName = nameof(Names.Property),
                ExpectedPath = $"{typeof(Names).FullName}.{nameof(Names.Property)}"
            };

            Run(Test, case1, case2);

            void Test(TestCase testCase)
            {
                AreEqual(testCase.ExpectedName, testCase.Name);
                AreEqual(testCase.ExpectedPath, testCase.Name.Path);
            }
        }

        [TestMethod]
        public void MethodNameTest()
        {
            var case1 = new TestCase
            {
                Name = new Name(typeof(Names).GetConstructor(new Type[0])),
                ExpectedName = "ctor",
                ExpectedPath = $"{typeof(Names).FullName}.ctor"
            };

            var case2 = new TestCase
            {
                Name = new Name(typeof(Names).GetConstructor(new[] {typeof(int), typeof(int)})),
                ExpectedName = $"ctor({typeof(int).FullName},{typeof(int).FullName})",
                ExpectedPath = $"{typeof(Names).FullName}.ctor({typeof(int).FullName},{typeof(int).FullName})"
            };

            var case3 = new TestCase
            {
                Name = new Name(typeof(Names).GetMethod(nameof(Names.Method), 0, new Type[0])),
                ExpectedName = nameof(Names.Method),
                ExpectedPath = $"{typeof(Names).FullName}.{nameof(Names.Method)}"
            };

            var case4 = new TestCase
            {
                Name = new Name(typeof(Names).GetMethod(nameof(Names.Method), 0, new[] {typeof(int), typeof(int)})),
                ExpectedName = $"{nameof(Names.Method)}({typeof(int).FullName},{typeof(int).FullName})",
                ExpectedPath = $"{typeof(Names).FullName}.{nameof(Names.Method)}({typeof(int).FullName},{typeof(int).FullName})"
            };

            var case5 = new TestCase
            {
                Name = new Name(typeof(Names).GetMethod(nameof(Names.Method), 1, new []{Type.MakeGenericMethodParameter(0)})),
                ExpectedName = $"{nameof(Names.Method)}(T)",
                ExpectedPath = $"{typeof(Names).FullName}.{nameof(Names.Method)}(T)"
            };
            
            Run(Test, case1, case2, case3, case4, case5);

            void Test(TestCase testCase)
            {
                AreEqual(testCase.ExpectedName, testCase.Name);
                AreEqual(testCase.ExpectedPath, testCase.Name.Path);
            }
        }

        [TestMethod]
        public void ParameterNameTest()
        {
            Run(Test);

            void Test()
            {
                var name = new Name(typeof(Names).GetMethod(nameof(Names.Method), 0, new[] {typeof(int), typeof(int)}).GetParameters().First());

                AreEqual("a", name);
                AreEqual($"{typeof(Names).FullName}.{nameof(Names.Method)}.a", name.Path);
            }
        }

        private class TestCase
        {
            public Name Name { get; set; }

            public string ExpectedName { get; set; }

            public string ExpectedPath { get; set; }
        }
    }
}