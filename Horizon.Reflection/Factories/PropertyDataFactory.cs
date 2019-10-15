using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a factory for converting <see cref="PropertyInfo"/> to <see cref="PropertyData"/>.
    /// </summary>
    internal sealed class PropertyDataFactory : BaseDataFactory<PropertyInfo, PropertyData>
    {
        /// <summary>
        /// The single instance of <see cref="PropertyDataFactory"/>.
        /// </summary>
        internal static PropertyDataFactory Instance { get; } = new PropertyDataFactory();

        /// <summary>
        /// Creates a new instance of <see cref="PropertyDataFactory"/>.
        /// </summary>
        private PropertyDataFactory()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="PropertyData"/>.
        /// </summary>
        /// <param name="propertyInfo">Property info.</param>
        /// <param name="declaringType">Declaring type.</param>
        /// <returns>New instance of <see cref="PropertyData"/>.</returns>
        protected override PropertyData Constructor(PropertyInfo propertyInfo, TypeData declaringType)
        {
            return new PropertyData(propertyInfo, declaringType);
        }

        /// <summary>
        /// Gets all <see cref="PropertyInfo"/> in the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="bindingFlags">Binding flags.</param>
        /// <returns>Collection of <see cref="PropertyInfo"/> defined by the specified <see cref="Type"/>.</returns>
        protected override IEnumerable<PropertyInfo> GetMemberInfos(Type type, BindingFlags bindingFlags)
        {
            return type.GetProperties(bindingFlags);
        }

        /// <summary>
        /// Gets all <see cref="PropertyData"/> in the specified <see cref="TypeData"/>.
        /// </summary>
        /// <param name="typeData">Type data.</param>
        /// <param name="modifierFlags">Modifier flags.</param>
        /// <returns><see cref="PropertyData"/> defined in the base type.</returns>
        protected override IEnumerable<PropertyData> GetBaseData(TypeData typeData, ModifierFlags modifierFlags)
        {
            return typeData.BaseType.Properties.Where(propertyData => propertyData.Get | modifierFlags || propertyData.Set | modifierFlags);
        }

        /// <summary>
        /// Are the two <see cref="PropertyData"/> objects equivalent?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <returns>True if the two <see cref="PropertyData"/> objects are equivalent; otherwise, false.</returns>
        protected override bool AreEquivalent(PropertyData lhs, PropertyData rhs)
        {
            return lhs.Name == rhs.Name;
        }
    }
}