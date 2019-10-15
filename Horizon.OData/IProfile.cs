using System.Collections.Generic;
using Horizon.Reflection;

namespace Horizon.OData
{
    public interface IProfile
    {
        TypeData ControllerType { get; }
        
        void AddController(IController controller);

        IEnumerable<IController> GetControllers();
    }
}