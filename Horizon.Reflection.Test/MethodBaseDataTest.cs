using System.Linq;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class MethodBaseDataTest : BaseTest
    {
        [TestMethod]
        public void MethodInvokeTest()
        {
            var message = new Message();
            var messageHelper = new MessageHelper();

            var case1 = new InvokeTestCase
            {
                MethodBaseData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.Append)),
                Void = true,
                Parameters = new object[] {"string"},
                Value = "string"
            };

            var case2 = new InvokeTestCase
            {
                MethodBaseData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.GetCharacters)),
                Void = false,
                Parameters = new object[0],
                ReturnValue = case1.Value.ToCharArray()
            };

            Run(Test, case1, case2);

            void Test(InvokeTestCase testCase)
            {
                if (!testCase.Void)
                {
                    SequenceEquals((char[]) testCase.ReturnValue, testCase.MethodBaseData.Invoke<char[]>(message, testCase.Parameters));
                }
                else
                {
                    testCase.MethodBaseData.Invoke(message, testCase.Parameters);
                    AreEqual(testCase.Value, message.Value);
                }
            }
        }

        [TestMethod]
        public void MethodParametersTest()
        {
          var messageHelper = new MessageHelper();
          
            var case1 = new ParametersTestCase
            {
                MethodBaseData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.Append)),
                ParameterNames = new[] {new Name(typeof(string))}
            };

            var case2 = new ParametersTestCase
            {
                MethodBaseData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.GetCharacters)),
                ParameterNames = new Name[0]
            };

            var case3 = new ParametersTestCase
            {
                MethodBaseData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.AppendMessage)),
                ParameterNames = new[] {new Name(messageHelper.AppendMessage.GetParameters().First().ParameterType)}
            };

            Run(Test, case1, case2, case3);

            void Test(ParametersTestCase testCase)
            {
                SequenceEquals(testCase.ParameterNames, testCase.MethodBaseData.Parameters.Select(parameter => parameter.ParameterType.Name));
            }
        }

        private class InvokeTestCase
        {
            public MethodBaseData MethodBaseData { get; set; }

            public bool Void { get; set; }

            public object[] Parameters { get; set; }

            public object ReturnValue { get; set; }

            public string Value { get; set; }
        }

        private class ParametersTestCase
        {
            public MethodBaseData MethodBaseData { get; set; }

            public Name[] ParameterNames { get; set; }
        }
    }
}