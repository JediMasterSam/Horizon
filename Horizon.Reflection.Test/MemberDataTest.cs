using System;
using System.Linq;
using System.Reflection;
using Horizon.Diagnostics;
using Horizon.Reflection.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test
{
    [TestClass]
    public class MemberDataTest : BaseTest
    {
        [TestMethod]
        public void MemberAttributeTest()
        {
            var attributesType = typeof(Attributes).GetTypeData();
            var baseAttributesType = typeof(BaseAttributes).GetTypeData();

            var case1 = new AttributeTestCase
            {
                MemberData = attributesType,
                Local = false
            };

            var case2 = new AttributeTestCase
            {
                MemberData = baseAttributesType,
                Local = true
            };

            var case3 = new AttributeTestCase
            {
                MemberData = attributesType.Properties.Get(new Name(typeof(Attributes).GetProperty(nameof(Attributes.Property)))),
                Local = false
            };

            var case4 = new AttributeTestCase
            {
                MemberData = baseAttributesType.Properties.Get(new Name(typeof(BaseAttributes).GetProperty(nameof(BaseAttributes.Property)))),
                Local = true
            };

            var case5 = new AttributeTestCase
            {
                MemberData = attributesType.Methods.Get(new Name(typeof(Attributes).GetMethod(nameof(Attributes.Method), 0, new[] {typeof(int)}))),
                Local = false
            };

            var case6 = new AttributeTestCase
            {
                MemberData = baseAttributesType.Methods.Get(new Name(typeof(BaseAttributes).GetMethod(nameof(BaseAttributes.Method), 0, new[] {typeof(int)}))),
                Local = true
            };

            var case7 = new AttributeTestCase
            {
                MemberData = attributesType.Methods.Get(new Name(typeof(Attributes).GetMethod(nameof(Attributes.Method), 0, new[] {typeof(int)}))).Parameters.First(),
                Local = false
            };

            var case8 = new AttributeTestCase
            {
                MemberData = baseAttributesType.Methods.Get(new Name(typeof(BaseAttributes).GetMethod(nameof(BaseAttributes.Method), 0, new[] {typeof(int)}))).Parameters.First(),
                Local = true
            };

            var case9 = new AttributeTestCase
            {
                MemberData = attributesType.Assembly,
                Local = false
            };

            Run(Test, case1, case2, case3, case4, case5, case6, case7, case8, case9);

            void Test(AttributeTestCase testCase)
            {
                IsTrue(testCase.MemberData.Attributes.Contains<GlobalAttribute>());

                if (testCase.Local)
                {
                    IsTrue(testCase.MemberData.Attributes.Contains<LocalAttribute>());
                }
                else
                {
                    IsFalse(testCase.MemberData.Attributes.Contains<LocalAttribute>());
                }
            }
        }

        [TestMethod]
        public void MemberNameTest()
        {
            var case1 = new NameTestCase(typeof(Names));
            var case2 = new NameTestCase(typeof(Names).GetMethod(nameof(Names.Method), 0, new Type[0]));
            var case3 = new NameTestCase(typeof(Names).GetProperty(nameof(Names.Property)));
            var case4 = new NameTestCase(typeof(Names).GetMethod(nameof(Names.Method), 0, new[] {typeof(int), typeof(int)}).GetParameters().First());
            var case5 = new NameTestCase(typeof(Names).Assembly);

            Run(Test, case1, case2, case3, case4, case5);

            void Test(NameTestCase testCase)
            {
                AreEqual(testCase.Name, testCase.MemberData.Name);
            }
        }

        private class AttributeTestCase
        {
            public MemberData MemberData { get; set; }

            public bool Local { get; set; }
        }

        private class NameTestCase
        {
            public MemberData MemberData { get; }

            public Name Name { get; }

            public NameTestCase(Type type)
            {
                MemberData = type.GetTypeData();
                Name = new Name(type);
            }

            public NameTestCase(MemberInfo memberInfo)
            {
                MemberData = memberInfo.DeclaringType.GetTypeData().Properties.Get(new Name(memberInfo));
                Name = new Name(memberInfo);
            }

            public NameTestCase(MethodBase methodBase)
            {
                MemberData = methodBase.DeclaringType.GetTypeData().Methods.Get(new Name(methodBase));
                Name = new Name(methodBase);
            }

            public NameTestCase(ParameterInfo parameterInfo)
            {
                var methodInfo = parameterInfo.Member as MethodInfo;
                if (methodInfo == null) return;
                
                Name = new Name(parameterInfo);
                MemberData = methodInfo.DeclaringType.GetTypeData().Methods.Get(new Name(methodInfo)).Parameters.FirstOrDefault(parameter => parameter.Name == Name.Value);
            }

            public NameTestCase(Assembly assembly)
            {
                MemberData = assembly.GetAssemblyData();
                Name = new Name(assembly);
            }
        }
    }
}