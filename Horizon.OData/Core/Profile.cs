using Horizon.Reflection;

namespace Horizon.OData
{
    public abstract class Profile<TController> : IProfile where TController : IController
    {
        protected Profile()
        {
            ControllerType = typeof(TController).GetTypeData();
        }

        public TypeData ControllerType { get; }
    }
}