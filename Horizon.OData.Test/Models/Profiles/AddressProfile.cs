using System.Collections.Generic;
using Horizon.Reflection;

namespace Horizon.OData.Test.Models.Profiles
{
    public class AddressProfile : IProfile
    {
        private readonly List<IController> _controllers;

        public AddressProfile()
        {
            _controllers = new List<IController>();

            ControllerType = typeof(IAddressController).GetTypeData();
        }

        public TypeData ControllerType { get; }

        public void AddController(IController controller)
        {
            _controllers.Add(controller);
        }

        public IEnumerable<IController> GetControllers()
        {
            return _controllers;
        }
    }

    public interface IAddressController : IController
    {
    }
}