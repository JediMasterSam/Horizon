using Horizon.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.Reflection.Test.Models
{
    [TestClass]
    public class MemberDataTest : BaseTest
    {
        [TestMethod]
        public void GetAttributesTest()
        {
            Run(Test);
            
            void Test()
            {
                var assembly = GetType().Assembly.GetAssemblyData();

                IsTrue(assembly.HasAttribute<GlobalAttribute>());
                IsTrue(assembly.TryGetAttribute<GlobalAttribute>(out var globalAttribute));
                IsNotNull(globalAttribute);
                IsNotEmpty(assembly.GetAttributes<GlobalAttribute>());
            }
        }
    }
}