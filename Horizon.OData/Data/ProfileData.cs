using System;
using System.Collections.Generic;
using Horizon.OData.Factories;
using Horizon.Reflection;

namespace Horizon.OData
{
    public sealed class ProfileData
    {
        private readonly Lazy<string> _name;
        
        private readonly List<ControllerData> _controllers;

        internal ProfileData(IProfile profile, ApiData apiData)
        {
            _name = new Lazy<string>(() => NameFactory.GetProfileName(this));
            _controllers = new List<ControllerData>();

            ProfileType = profile.GetTypeData();
            ControllerType = profile.ControllerType;
            ApiData = apiData;
        }

        public TypeData ProfileType { get; }

        public TypeData ControllerType { get; }

        public ApiData ApiData { get; }

        public string Name => _name.Value;

        public IReadOnlyList<ControllerData> Controllers => _controllers;

        internal bool AddController(ControllerData controller)
        {
            if (!controller.ControllerType.IsAssignableTo(ControllerType)) return false;

            _controllers.Add(controller);
            controller.Profile = this;

            return true;
        }
    }
}