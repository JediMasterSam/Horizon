using System;
using System.Collections.Generic;
using Horizon.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class IsNullableTest : BaseTest
    {
        [TestMethod]
        public void NullableTest()
        {
            Run(Test);
            
            void Test()
            {
                IsTrue(new IsNullable(typeof(int?)));
                IsTrue(new IsNullable(typeof(List<>)));
            }
        }

        [TestMethod]
        public void IsNotNullableTest()
        {
            Run(Test);
            
            void Test()
            {
                IsFalse(new IsNullable(typeof(int)));
                IsFalse(new IsNullable(typeof(DateTime)));
            }
        }
    }
}