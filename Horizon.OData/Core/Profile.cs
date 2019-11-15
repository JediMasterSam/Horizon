using Horizon.Reflection;

namespace Horizon.OData
{
    public abstract class Profile<TController> : IProfile
    {
        protected Profile()
        {
            ControllerType = typeof(TController).GetTypeData();
        }

        public TypeData ControllerType { get; }
    }
}