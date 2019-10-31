using System;
using System.Collections.Generic;
using Horizon.Reflection;

namespace Horizon.OData
{
    public sealed class ProfileData
    {
        private readonly Lazy<string> _routeName;
        
        private readonly List<ControllerData> _controllers;

        internal ProfileData(IProfile profile, ApiData apiData)
        {
            _routeName = new Lazy<string>(() => GetRouteName(ProfileType));
            _controllers = new List<ControllerData>();

            ProfileType = profile.GetTypeData();
            ControllerType = profile.ControllerType;
            ApiData = apiData;
        }

        public TypeData ProfileType { get; }

        public TypeData ControllerType { get; }

        public ApiData ApiData { get; }

        public string RouteName => _routeName.Value;

        public IReadOnlyList<ControllerData> Controllers => _controllers;

        internal bool AddController(ControllerData controller)
        {
            if (!controller.ControllerType.IsAssignableTo(ControllerType)) return false;

            _controllers.Add(controller);
            controller.Profile = this;

            return true;
        }

        private static string GetRouteName(MemberData profileType)
        {
            const string suffix = "Profile";

            if (profileType.Name.EndsWith(suffix))
            {
                return profileType.Name.Substring(0, profileType.Name.Length - suffix.Length);
            }

            throw new ProfileException($"The name '{profileType.Name}' does have the suffix '{suffix}'.", profileType);
        }
    }
}