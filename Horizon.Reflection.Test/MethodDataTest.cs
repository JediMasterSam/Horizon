using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class MethodDataTest : BaseTest
    {
        [TestMethod]
        public void MethodReturnTypeTest()
        {
            var messageHelper = new MessageHelper();

            var case1 = new ReturnTypeTestCase
            {
                MethodData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.Append)),
                ReturnTypeName = new Name(messageHelper.Append.ReturnType)
            };

            var case2 = new ReturnTypeTestCase
            {
                MethodData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.GetCharacters)),
                ReturnTypeName = new Name(messageHelper.GetCharacters.ReturnType)
            };

            var case3 = new ReturnTypeTestCase
            {
                MethodData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.AppendMessage)),
                ReturnTypeName = new Name(messageHelper.AppendMessage.ReturnType)
            };

            var case4 = new ReturnTypeTestCase
            {
                MethodData = messageHelper.MessageType.Methods.Get(new Name(messageHelper.AppendMessage)).MakeGenericMethod(typeof(Message)),
                ReturnTypeName = new Name(typeof(Message))
            };

            Run(Test, case1, case2, case3, case4);

            void Test(ReturnTypeTestCase testCase)
            {
                AreEqual(testCase.ReturnTypeName.Path, testCase.MethodData.ReturnType.Name.Path);
            }
        }

        [TestMethod]
        public void GenericMethodTest()
        {
            Run(Test);

            void Test()
            {
                var messageHelper = new MessageHelper();
                var appendMessage = messageHelper.MessageType.Methods.Get(new Name(messageHelper.AppendMessage));
                var constructedAppendMessage = appendMessage.MakeGenericMethod(typeof(Message));

                IsTrue(appendMessage.IsGenericMethod);
                IsTrue(constructedAppendMessage.IsGenericMethod);
                AreEqual(appendMessage, constructedAppendMessage.GenericMethodDefinition);
                SequenceEquals(new[] {messageHelper.MessageType}, appendMessage.GenericArguments.First().GenericParameterConstraints);

                var genericListAdd = typeof(List<>).GetMethod(nameof(IList.Add));
                var constructedListAdd = typeof(List<int>).GetMethod(nameof(IList.Add));

                var genericAdd = typeof(List<>).GetTypeData().Methods.Get(new Name(genericListAdd));
                var constructedAdd = typeof(List<int>).GetTypeData().Methods.Get(new Name(constructedListAdd));

                IsFalse(genericAdd.IsGenericMethod);
                IsFalse(constructedAdd.IsGenericMethod);
            }
        }

        private class ReturnTypeTestCase
        {
            public MethodData MethodData { get; set; }

            public Name ReturnTypeName { get; set; }
        }
    }
}