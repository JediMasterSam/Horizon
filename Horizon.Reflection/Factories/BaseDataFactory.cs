using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    /// <summary>
    /// Represents a factory for converting <see cref="TMemberInfo"/> to <see cref="TMemberData"/>.
    /// </summary>
    /// <typeparam name="TMemberInfo">Member.</typeparam>
    /// <typeparam name="TMemberData">Data.</typeparam>
    internal abstract class BaseDataFactory<TMemberInfo, TMemberData> where TMemberInfo : MemberInfo where TMemberData : MemberData
    {
        /// <summary>
        /// Reflection search conditions.
        /// </summary>
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        /// <summary>
        /// Gets all <see cref="TMemberInfo"/> in the specified type and converts them to <see cref="TMemberData"/>.
        /// </summary>
        /// <param name="typeData">Type data.</param>
        /// <returns>Collection of <see cref="TMemberData"/> in defined by the specified <see cref="Type"/>.</returns>
        internal IReadOnlyList<TMemberData> Get(TypeData typeData)
        {
            if (typeData.BaseType == null)
            {
                return GetMemberInfos(typeData, Flags).Select(memberInfo => Constructor(memberInfo, typeData)).ToArray();
            }

            var members = GetBaseData(typeData, typeData.Assembly == typeData.BaseType.Assembly ? ModifierFlags.Family | ModifierFlags.Internal : ModifierFlags.Family).ToList();
            var count = members.Count;

            foreach (var memberInfo in GetMemberInfos(typeData, Flags))
            {
                var found = false;
                var member = Constructor(memberInfo, typeData);

                for (var index = 0; index < count; index++)
                {
                    if (!AreEquivalent(member, members[index])) continue;

                    members[index] = member;
                    found = true;
                    break;
                }

                if (!found)
                {
                    members.Add(member);
                }
            }

            return members;
        }

        /// <summary>
        /// Creates a new instance of <see cref="TMemberData"/>.
        /// </summary>
        /// <param name="memberInfo">Member info.</param>
        /// <param name="declaringType">Declaring type.</param>
        /// <returns>New instance of <see cref="TMemberData"/>.</returns>
        protected abstract TMemberData Constructor(TMemberInfo memberInfo, TypeData declaringType);

        /// <summary>
        /// Gets all <see cref="TMemberInfo"/> in the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="bindingFlags">Binding flags.</param>
        /// <returns>Collection of <see cref="TMemberInfo"/> defined by the specified <see cref="Type"/>.</returns>
        protected abstract IEnumerable<TMemberInfo> GetMemberInfos(Type type, BindingFlags bindingFlags);

        /// <summary>
        /// Gets all base <see cref="TMemberData"/> in the specified <see cref="TypeData"/>.
        /// </summary>
        /// <param name="typeData">Type.</param>
        /// <param name="modifierFlags">Modifier flags.</param>
        /// <returns>Members defined in the base type.</returns>
        protected abstract IEnumerable<TMemberData> GetBaseData(TypeData typeData, ModifierFlags modifierFlags);

       
        /// <summary>
        /// Is the specified left hand side <see cref="TMemberData"/> equivalent to the specified right hand side <see cref="TMemberData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="TMemberData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="TMemberData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="TMemberData"/> is equivalent to the specified right hand side <see cref="TMemberData"/>; otherwise, false.</returns>
        protected abstract bool AreEquivalent(TMemberData lhs, TMemberData rhs);
    }
}