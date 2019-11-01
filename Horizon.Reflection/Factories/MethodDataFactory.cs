using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a factory for converting <see cref="MethodInfo"/> to <see cref="MethodData"/>.
    /// </summary>
    internal sealed class MethodDataFactory : BaseDataFactory<MethodInfo, MethodData>
    {
        /// <summary>
        /// The single instance of <see cref="MethodDataFactory"/>.
        /// </summary>
        internal static MethodDataFactory Instance { get; } = new MethodDataFactory();

        /// <summary>
        /// Creates a new instance of <see cref="MethodDataFactory"/>.
        /// </summary>
        private MethodDataFactory()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="MethodData"/>.
        /// </summary>
        /// <param name="methodInfo">Method info.</param>
        /// <param name="declaringType">Declaring type.</param>
        /// <returns>New instance of <see cref="MethodData"/>.</returns>
        protected override MethodData Constructor(MethodInfo methodInfo, TypeData declaringType)
        {
            return new MethodData(methodInfo, declaringType);
        }

        /// <summary>
        /// Gets all <see cref="MethodInfo"/> in the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="bindingFlags">Binding flags.</param>
        /// <returns>Collection of <see cref="MethodInfo"/> defined by the specified <see cref="Type"/>.</returns>
        protected override IEnumerable<MethodInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetMethods(bindingFlags).Where(methodInfo => !methodInfo.IsSpecialName);
        }

        /// <summary>
        /// Gets all base <see cref="MethodData"/> in the specified <see cref="TypeData"/>.
        /// </summary>
        /// <param name="typeData">Type data.</param>
        /// <param name="modifierFlags">Modifier flags.</param>
        /// <returns><see cref="MethodData"/> defined in the base type.</returns>
        protected override IEnumerable<MethodData> GetBaseData(TypeData typeData, ModifierFlags modifierFlags)
        {
            return typeData.BaseType.Methods.Where(methodData => methodData | modifierFlags);
        }

        /// <summary>
        /// Is the specified left hand side <see cref="MethodData"/> equivalent to the specified right hand side <see cref="MethodData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="MethodData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="MethodData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="MethodData"/> is equivalent to the specified right hand side <see cref="MethodData"/>; otherwise, false.</returns>
        protected override bool AreEquivalent(MethodData lhs, MethodData rhs)
        {
            return lhs.Name == rhs.Name &&
                   lhs.Parameters.Count == rhs.Parameters.Count &&
                   !lhs.Parameters.Where((parameterData, index) => parameterData.ParameterType != rhs.Parameters[index].ParameterType).Any();
        }
    }
}