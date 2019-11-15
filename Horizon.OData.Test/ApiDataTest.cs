using System.Linq;
using Horizon.Diagnostics;
using Horizon.OData.Test.Models.Profiles;
using Horizon.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Horizon.OData.Test
{
    [TestClass]
    public class ApiDataTest : BaseTest
    {
        public ApiDataTest()
        {
            ApiData = new ApiData(GetType().Assembly);
        }

        private ApiData ApiData { get; }

        [TestMethod]
        public void FindProfilesTest()
        {
            Run(Test);

            void Test()
            {
                if (AreEqual(1, ApiData.Profiles.Count))
                {
                    AreEqual(typeof(AddressProfile), ApiData.Profiles.First().ProfileType);
                }
            }
        }

        [TestMethod]
        public void FindControllersTest()
        {
            Run(Test);
            
            void Test()
            {
                if (!AreEqual(2, ApiData.Controllers.Count)) return;
                
                var types = ApiData.Controllers.Select(controller => controller.GetTypeData()).ToArray();

                Contains(types, typeof(Models.Controllers.V1.AddressController).GetTypeData());
                Contains(types, typeof(Models.Controllers.V2.AddressController).GetTypeData());
            }
        }
    }
}