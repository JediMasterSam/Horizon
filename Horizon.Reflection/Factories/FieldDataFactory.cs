using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a factory for converting <see cref="FieldInfo"/> to <see cref="FieldData"/>.
    /// </summary>
    internal sealed class FieldDataFactory : BaseDataFactory<FieldInfo, FieldData>
    {
        /// <summary>
        /// The single instance of <see cref="FieldDataFactory"/>.
        /// </summary>
        internal static FieldDataFactory Instance { get; } = new FieldDataFactory();

        /// <summary>
        /// Creates a new instance of <see cref="FieldDataFactory"/>.
        /// </summary>
        private FieldDataFactory()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FieldData"/>.
        /// </summary>
        /// <param name="fieldInfo">Field info.</param>
        /// <param name="declaringType">Declaring type.</param>
        /// <returns>New instance of <see cref="FieldData"/>.</returns>
        protected override FieldData Constructor(FieldInfo fieldInfo, TypeData declaringType)
        {
            return new FieldData(fieldInfo, declaringType);
        }

        /// <summary>
        /// Gets all <see cref="FieldInfo"/> in the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="bindingFlags">Binding flags.</param>
        /// <returns>Collection of <see cref="FieldInfo"/> defined by the specified <see cref="Type"/>.</returns>
        protected override IEnumerable<FieldInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetFields(bindingFlags).Where(fieldInfo => fieldInfo.Name[0] != '<');
        }

        /// <summary>
        /// Gets all base <see cref="FieldData"/> in the specified <see cref="TypeData"/>.
        /// </summary>
        /// <param name="typeData">Type data.</param>
        /// <param name="modifierFlags">Modifier flags.</param>
        /// <returns><see cref="FieldData"/> defined in the base type.</returns>
        protected override IEnumerable<FieldData> GetBaseData(TypeData typeData, ModifierFlags modifierFlags)
        {
            return typeData.BaseType.Fields.Where(fieldData => fieldData | modifierFlags);
        }

        /// <summary>
        /// Are the two <see cref="FieldData"/> objects equivalent?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>True if the two <see cref="FieldData"/> objects are equivalent; otherwise, false.</returns>
        protected override bool AreEquivalent(FieldData lhs, FieldData rhs)
        {
            return lhs.Name == rhs.Name;
        }
    }
}