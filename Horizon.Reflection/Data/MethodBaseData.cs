using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a cached <see cref="MethodBase"/>.
    /// </summary>
    public abstract class MethodBaseData : ModifierData
    {
        /// <summary>
        /// Cached <see cref="MethodBase"/>.
        /// </summary>
        private readonly MethodBase _methodBase;

        /// <summary>
        /// Collection of every <see cref="AttributeData"/> applied to the current <see cref="MethodBaseData"/>.
        /// </summary>
        private readonly Lazy<IReadOnlyList<AttributeData>> _attributes;

        /// <summary>
        /// The XML summary given to the current <see cref="TypeData"/>.
        /// </summary>
        private readonly Lazy<string> _description;

        /// <summary>
        /// Creates a new instance of <see cref="MethodBaseData"/>.
        /// </summary>
        /// <param name="methodBase">Method base.</param>
        /// <param name="declaringType">Declaring type.</param>
        protected internal MethodBaseData(MethodBase methodBase, TypeData declaringType) : base(methodBase.GetModifierFlags(), methodBase.Name, declaringType)
        {
            _methodBase = methodBase;
            _attributes = new Lazy<IReadOnlyList<AttributeData>>(() => _methodBase.GetCustomAttributes(true).Select(value => new AttributeData(value, value.GetType(), this)).ToArray());
            _description = new Lazy<string>(() => DeclaringType.Assembly.XmlDocumentation.GetSummary(this));

            DeclaringType = declaringType;
            Parameters = methodBase.GetParameters().Select(parameterInfo => new ParameterData(parameterInfo, this)).ToArray();
        }

        ///<inheritdoc cref="_attributes"/>
        public override IReadOnlyList<AttributeData> Attributes => _attributes.Value;
        
        ///<inheritdoc cref="_description"/>
        public string Description => _description.Value;

        /// <summary>
        /// The declaring <see cref="TypeData"/> of the current <see cref="MethodBaseData"/>.
        /// </summary>
        public TypeData DeclaringType { get; }

        /// <summary>
        /// Collection of every <see cref="ParameterInfo"/> declared in the current <see cref="MethodBaseData"/>.
        /// </summary>
        public IReadOnlyList<ParameterData> Parameters { get; }

        /// <summary>
        /// Implicitly converts the specified <see cref="MethodBaseData"/> to <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="methodBaseData">Method base data.</param>
        /// <returns>Cached <see cref="MethodBase"/>.</returns>
        public static implicit operator MethodBase(MethodBaseData methodBaseData)
        {
            return methodBaseData._methodBase;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="MethodBaseData"/> equal the specified right hand side <see cref="MethodBaseData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="MethodBaseData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="MethodBaseData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="MethodBaseData"/> equals the specified right hand side <see cref="MethodBaseData"/>; otherwise, false.</returns>
        protected static bool Equals(MethodBaseData lhs, MethodBaseData rhs)
        {
            return lhs.Path == rhs.Path &&
                   lhs.Parameters.Count == rhs.Parameters.Count &&
                   !lhs.Parameters.Where((parameter, index) => parameter.ParameterType != rhs.Parameters[index].ParameterType).Any();
        }
    }
}