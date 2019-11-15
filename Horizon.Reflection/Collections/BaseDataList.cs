using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    public abstract class BaseDataList<TMemberInfo, TMemberData> : IReadOnlyList<TMemberData> where TMemberInfo : MemberInfo where TMemberData : ModifierData
    {
        private readonly TypeData _declaringType;

        private readonly Lazy<TMemberData[]> _members;

        protected BaseDataList(TypeData declaringType)
        {
            _declaringType = declaringType;
            _members = new Lazy<TMemberData[]>(() => Initialize().ToArray());
        }

        public int Count => _members.Value.Length;

        public TMemberData this[int index] => _members.Value[index];

        public IEnumerator<TMemberData> GetEnumerator()
        {
            return _members.Value.OfType<TMemberData>().GetEnumerator();
        }

        public TMemberData Get(string name)
        {
            return _members.Value.FirstOrDefault(member => string.Equals(name, member.Name));
        }

        protected abstract IEnumerable<TMemberInfo> GetMemberInfos(Type type, BindingFlags bindingFlags);

        protected abstract IEnumerable<TMemberData> GetMembers(TypeData typeData);

        protected abstract TMemberData Constructor(TMemberInfo memberInfo, TypeData declaringType);

        private IEnumerable<TMemberData> Initialize()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

            try
            {
                if (_declaringType.BaseType == null)
                {
                    return GetMemberInfos(_declaringType, flags).Select(memberInfo => Constructor(memberInfo, _declaringType));
                }

                var modiferFlags = _declaringType.Assembly.Equals(_declaringType.BaseType.Assembly) ? ModifierFlags.Family | ModifierFlags.Internal : ModifierFlags.Family;
                var members = new Dictionary<string, TMemberData>();

                foreach (var member in GetMembers(_declaringType.BaseType).Where(member => member.Modifier.Flags | modiferFlags))
                {
                    members[member.Name] = member;
                }

                foreach (var memberInfo in GetMemberInfos(_declaringType, flags))
                {
                    var member = Constructor(memberInfo, _declaringType);

                    if (members.TryGetValue(member.Name, out var baseMember))
                    {
                        members[baseMember.Name] = Constructor(memberInfo, baseMember.DeclaringType);
                    }
                    else
                    {
                        members[member.Name] = member;
                    }
                }

                return members.Values;
            }
            catch
            {
                return Enumerable.Empty<TMemberData>();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}