using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Diagnostics.Test
{
    [TestClass]
    public class ExceptionTest : BaseTest
    {
        [TestMethod]
        public void Expect()
        {
            var case1 = new TestCase
            {
                Method = NoException,
                ExceptionType = null,
            };

            var case2 = new TestCase
            {
                Method = ThrowsException,
                ExceptionType = typeof(ArgumentOutOfRangeException),
            };

            Run(Test, case1, case2);

            void Test(TestCase testCase)
            {
                Expect(testCase.ExceptionType);
                testCase.Method.Invoke();
            }
        }

        private static void ThrowsException()
        {
            var list = new List<List<int>> {new List<int>()};
            list[1].Add(0);
        }

        private static void NoException()
        {
        }

        private class TestCase
        {
            public Action Method { get; set; }

            public Type ExceptionType { get; set; }
        }
    }
}