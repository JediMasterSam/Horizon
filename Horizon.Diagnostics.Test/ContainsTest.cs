﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Diagnostics.Test
{
    [TestClass]
    public class ContainsTest : BaseTest
    {
        [TestMethod]
        public void Contains()
        {
            Run(Test);

            void Test()
            {
                Contains(new[] {1, 2, 3}, 3);
                Contains("test", 's');
            }
        }

        [TestMethod]
        public void DoesNotContain()
        {
            Run(Test);

            void Test()
            {
                DoesNotContain(new[] {1, 2, 3}, 4);
                Contains("test", 'x');
            }
        }

        [TestMethod]
        public void SequenceEquals()
        {
            Run(Test);

            void Test()
            {
                SequenceEquals(new[] {1, 2, 3}, new[] {1, 2, 3});
                SequenceEquals("test", "test");
            }
        }

        [TestMethod]
        public void IsEmptyTest()
        {
            Run(Test);

            void Test()
            {
                IsEmpty(new int[0]);
                IsEmpty("");
            }
        }

        [TestMethod]
        public void IsNotEmptyTest()
        {
            Run(Test);
            
            void Test()
            {
                IsNotEmpty(new[] {1, 2, 3});
                IsNotEmpty("test");
            }
        }
    }
}