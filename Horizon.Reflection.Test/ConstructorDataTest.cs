using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class ConstructorDataTest : BaseTest
    {
        [TestMethod]
        public void ConstructorDefaultTest()
        {
            var constructors = new Constructors();
            var messageType = typeof(Message).GetTypeData();

            var case1 = new TestCase
            {
                ConstructorData = messageType.Constructors.Get(new Name(constructors.DefaultConstructor)),
                Default = true
            };

            var case2 = new TestCase
            {
                ConstructorData = messageType.Constructors.Get(new Name(constructors.NotDefaultConstructor)),
                Default = false
            };

            Run(Test, case1, case2);

            void Test(TestCase testCase)
            {
                AreEqual(testCase.Default, testCase.ConstructorData.IsDefault);
            }
        }

        [TestMethod]
        public void ConstructorInvokeTest()
        {
            var constructors = new Constructors();
            var messageType = typeof(Message).GetTypeData();
            
            var case1 = new InvokeTestCase
            {
                ConstructorData = messageType.Constructors.Get(new Name(constructors.DefaultConstructor)),
                Parameters = new object[0],
                Value = string.Empty
            };

            var case2 = new InvokeTestCase
            {
                ConstructorData = messageType.Constructors.Get(new Name(constructors.NotDefaultConstructor)),
                Parameters = new object[]{"test"},
                Value = "test"
            };
            
            Run(Test, case1, case2);

            void Test(InvokeTestCase testCase)
            {
                AreEqual(testCase.Value, testCase.ConstructorData.Invoke<Message>(testCase.Parameters).Value);
            }
        }

        private class TestCase
        {
            public ConstructorData ConstructorData { get; set; }

            public bool Default { get; set; }
        }
        
        private class InvokeTestCase
        {
            public ConstructorData ConstructorData { get; set; }

            public object[] Parameters { get; set; }

            public string Value { get; set; }
        }
    }
}