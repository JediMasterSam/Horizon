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
        
        /// <summary>
        /// Creates a new instance of <see cref="ConstructorData"/>.
        /// </summary>
        /// <param name="constructorInfo">Constructor info.</param>
        /// <param name="declaringType">Declaring type.</param>
        /// <returns>New instance of <see cref="ConstructorData"/>.</returns>
        protected override ConstructorData Constructor(ConstructorInfo constructorInfo, TypeData declaringType)
        {
            return new ConstructorData(constructorInfo, declaringType);
        }

        /// <summary>
        /// Gets all <see cref="ConstructorInfo"/> in the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="bindingFlags">Binding flags.</param>
        /// <returns>Collection of <see cref="ConstructorInfo"/> defined by the specified <see cref="Type"/>.</returns>
        protected override IEnumerable<ConstructorInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetConstructors(bindingFlags);
        }

        /// <summary>
        /// Gets all base <see cref="ConstructorData"/> in the specified <see cref="TypeData"/>.
        /// </summary>
        /// <param name="typeData">Type data.</param>
        /// <param name="modifierFlags">Modifier flags.</param>
        /// <returns><see cref="ConstructorData"/> defined in the base type.</returns>
        protected override IEnumerable<ConstructorData> GetBaseData(TypeData typeData, ModifierFlags modifierFlags)
        {
            return Enumerable.Empty<ConstructorData>();
        }

        /// <summary>
        /// Is the specified left hand side <see cref="ConstructorData"/> equivalent to the specified right hand side <see cref="ConstructorData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="ConstructorData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="ConstructorData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="ConstructorData"/> is equivalent to the specified right hand side <see cref="ConstructorData"/>; otherwise, false.</returns>
        protected override bool AreEquivalent(ConstructorData lhs, ConstructorData rhs)
        {
            return lhs.Parameters.Count == rhs.Parameters.Count &&
                   !lhs.Parameters.Where((parameterData, index) => parameterData.ParameterType != rhs.Parameters[index].ParameterType).Any();
        }
    }
}