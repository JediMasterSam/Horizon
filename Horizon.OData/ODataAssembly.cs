using System.Collections.Generic;
using System.Reflection;
using Horizon.Reflection;

namespace Horizon.OData
{
    public class ODataAssembly
    {
        public ODataAssembly(Assembly assembly)
        {
            var profiles = new List<IProfile>();
            var controllers = new List<IController>();

            var profileTypeData = typeof(IProfile).GetTypeData();
            var controllerTypeData = typeof(IController).GetTypeData();

            foreach (var typeData in assembly.GetAssemblyData().Types)
            {
                if (!(typeData & DefinitionFlags.Class)) continue;

                if (typeData.Implements(profileTypeData))
                {
                    if (typeData.TryCreate<IProfile>(null, out var profile))
                    {
                        profiles.Add(profile);
                    }
                }
                else if (typeData.Implements(controllerTypeData))
                {
                    if (typeData.TryCreate<IController>(null, out var controller))
                    {
                        controllers.Add(controller);
                    }
                }
            }

            var found = new bool[controllers.Count];

            foreach (var profile in profiles)
            {
                for (var index = 0; index < controllers.Count; index++)
                {
                    if (found[index] || !controllers[index].GetTypeData().IsAssignableTo(profile.ControllerType)) continue;

                    profile.AddController(controllers[index]);
                    found[index] = true;
                    break;
                }
            }

            Profiles = profiles;
            Controllers = controllers;
        }

        public IReadOnlyCollection<IProfile> Profiles { get; }

        public IReadOnlyCollection<IController> Controllers { get; }
    }
}