using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    internal sealed class ConstructorDataFactory : BaseDataFactory<ConstructorInfo, ConstructorData>
    {
        /// <summary>
        /// The single instance of <see cref="ConstructorDataFactory"/>.
        /// </summary>
        internal static ConstructorDataFactory Instance { get; } = new ConstructorDataFactory();

        /// <summary>
        /// Creates a new instance of <see cref="ConstructorDataFactory"/>.
        /// </summary>
        private ConstructorDataFactory()
        {
        }
        
        protected override ConstructorData Constructor(ConstructorInfo constructorInfo, TypeData declaringType)
        {
            return new ConstructorData(constructorInfo, declaringType);
        }

        protected override IEnumerable<ConstructorInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetConstructors(bindingFlags);
        }

        protected override IEnumerable<ConstructorData> GetBaseData(TypeData typeData, ModifierFlags modifierFlags)
        {
            return Enumerable.Empty<ConstructorData>();
        }

        protected override bool AreEquivalent(ConstructorData lhs, ConstructorData rhs)
        {
            return lhs.Parameters.Count == rhs.Parameters.Count &&
                   !lhs.Parameters.Where((parameterData, index) => parameterData.ParameterType != rhs.Parameters[index].ParameterType).Any();
        }
    }
}