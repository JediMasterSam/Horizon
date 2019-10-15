using System.Linq;
using System.Reflection;

namespace Horizon.Reflection
{
    public sealed class MethodData : MethodBaseData
    {
        private readonly MethodInfo _methodInfo;

        internal MethodData(MethodInfo methodInfo, TypeData declaringType) : base(methodInfo, declaringType)
        {
            _methodInfo = methodInfo;

            ReturnType = methodInfo.ReturnType.GetTypeData();
        }

        public TypeData ReturnType { get; }

        public static implicit operator MethodInfo(MethodData methodData)
        {
            return methodData._methodInfo;
        }

        public static bool operator ==(MethodData lhs, MethodData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;
            
            return Equals(lhs, rhs) && lhs.ReturnType == rhs.ReturnType;
        }

        public static bool operator !=(MethodData lhs, MethodData rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return obj is MethodData methodData && this == methodData;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Path.GetHashCode();
                hashCode = (hashCode * 397) ^ (ReturnType != null ? ReturnType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Parameters != null ? Parameters.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}