using System;
using System.Collections.Generic;
using System.Linq;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached generic argument of a generic <see cref="Type"/>.
    /// </summary>
    public sealed class GenericArgumentData : MemberData
    {
        /// <summary>
        /// Constraints on the <see cref="TypeData"/> of the current <see cref="GenericArgumentData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<TypeData>> _constraints;

        /// <summary>
        /// Creates a new instance of <see cref="GenericArgumentData"/>.
        /// </summary>
        /// <param name="genericArgumentType">Generic argument type.</param>
        /// <param name="declaringMember">Declaring member.</param>
        internal GenericArgumentData(Type genericArgumentType, MemberData declaringMember) : base(genericArgumentType.Name, declaringMember)
        {
            _constraints = new Lazy<IReadOnlyList<TypeData>>(() => ((Type) GenericArgumentType).GetGenericParameterConstraints().Select(type => type.GetTypeData()).ToArray());

            GenericArgumentType = genericArgumentType.GetTypeData();
        }

        /// <summary>
        /// The <see cref="TypeData"/> of the current <see cref="GenericArgumentData"/>.
        /// </summary>
        public TypeData GenericArgumentType { get; }
        
        ///<inheritdoc cref="_constraints"/>
        public IReadOnlyList<TypeData> Constraints => _constraints.Value;

        /// <summary>
        /// Collection of every <see cref="AttributeData"/> applied to the current <see cref="GenericArgumentData"/>.
        /// </summary>
        public override IReadOnlyList<AttributeData> Attributes => GenericArgumentType.Attributes;
    }
}