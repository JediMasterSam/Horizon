using System;
using System.Collections;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class DefinitionTest : BaseTest
    {
        [TestMethod]
        public void TypeDefinitionTest()
        {
            var case1 = new TestCase
            {
                Definition = new Definition(typeof(Names)),
                DefinitionFlags = DefinitionFlags.Class
            };

            var case2 = new TestCase
            {
                Definition = new Definition(typeof(IEnumerable)),
                DefinitionFlags = DefinitionFlags.Interface
            };

            var case3 = new TestCase
            {
                Definition = new Definition(typeof(DateTime)),
                DefinitionFlags = DefinitionFlags.Value
            };

            var case4 = new TestCase
            {
                Definition = new Definition(typeof(int)),
                DefinitionFlags = DefinitionFlags.Primitive
            };

            var case5 = new TestCase
            {
                Definition = new Definition(typeof(DefinitionFlags)),
                DefinitionFlags = DefinitionFlags.Enum
            };
            
            Run(Test, case1, case2, case3, case4, case5);

            void Test(TestCase testCase)
            {
                IsTrue(testCase.Definition.Flags & testCase.DefinitionFlags);
            }
        }
        
        private class TestCase
        {
            public Definition Definition { get; set; }
            
            public DefinitionFlags DefinitionFlags { get; set; }
        }
    }
}