using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Horizon.OData.Attributes;
using Horizon.Reflection;

namespace Horizon.OData
{
    public class ApiData
    {
        public ApiData(Assembly assembly)
        {
            var profiles = new List<ProfileData>();
            var controllers = new List<ControllerData>();

            var profileTypeData = typeof(IProfile).GetTypeData();
            var controllerTypeData = typeof(IController).GetTypeData();

            var assemblyData = assembly.GetAssemblyData();

            foreach (var typeData in assemblyData.Types)
            {
                if (!(typeData | (DefinitionFlags.Class | DefinitionFlags.DeconstructedGeneric)) || typeData | (ModifierFlags.Abstract | ModifierFlags.Static | ModifierFlags.NotPublic)) continue;

                if (typeData.Implements(profileTypeData))
                {
                    if (typeData.TryCreate<IProfile>(null, out var profile))
                    {
                        profiles.Add(new ProfileData(profile, this));
                    }
                }
                else if (typeData.Implements(controllerTypeData) && (!typeData.TryGetAttribute<DeprecatedAttribute>(out var deprecated) || !deprecated.Hide))
                {
                    controllers.Add(new ControllerData(typeData, this, deprecated != null));
                }
            }

            var found = new bool[controllers.Count];

            foreach (var profile in profiles)
            {
                for (var index = 0; index < controllers.Count; index++)
                {
                    if (!found[index] && profile.AddController(controllers[index]))
                    {
                        found[index] = true;
                    }
                }
            }

            Profiles = profiles;
            Controllers = controllers;
            Headers = assemblyData.GetAttributes<IHeader>().ToArray();
        }

        public IReadOnlyList<ProfileData> Profiles { get; }

        public IReadOnlyList<ControllerData> Controllers { get; }
        
        public IReadOnlyList<IParameter> Headers { get; }
    }
}