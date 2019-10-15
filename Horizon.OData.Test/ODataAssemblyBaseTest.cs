using System.Linq;
using Horizon.Diagnostics;
using Horizon.OData.Test.Models.Profiles;
using Horizon.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.OData.Test
{
    [TestClass]
    public class ODataAssemblyBaseTest : Diagnostics.BaseTest
    {
        public ODataAssemblyBaseTest()
        {
            ODataAssembly = new ODataAssembly(GetType().Assembly);
        }

        private ODataAssembly ODataAssembly { get; }

        [TestMethod]
        public void FindProfilesTest()
        {
            Run(Test);

            void Test()
            {
                if (AreEqual(1, ODataAssembly.Profiles.Count))
                {
                    AreEqual(typeof(AddressProfile), ODataAssembly.Profiles.First().GetType());
                }
            }
        }

        [TestMethod]
        public void FindControllersTest()
        {
            Run(Test);
            
            void Test()
            {
                if (!AreEqual(2, ODataAssembly.Controllers.Count)) return;
                
                var types = ODataAssembly.Controllers.Select(controller => controller.GetTypeData()).ToArray();

                Contains(types, typeof(Models.Controllers.V1.AddressController).GetTypeData());
                Contains(types, typeof(Models.Controllers.V2.AddressController).GetTypeData());
            }
        }
    }
}