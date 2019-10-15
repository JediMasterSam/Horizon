namespace Horizon.Reflection
{
    /// <summary>
    /// Represents the member information of a reflected value.
    /// </summary>
    public abstract class MemberData
    {
        /// <summary>
        /// Creates a new instance of <see cref="MemberData"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="path">Path.</param>
        protected internal MemberData(string name, string path)
        {
            Name = name;
            Path = path;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MemberData"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="declaringMember">Declaring member.</param>
        protected internal MemberData(string name, MemberData declaringMember)
        {
            Name = name;
            Path = $"{declaringMember.Path}.{name}";
        }

        /// <summary>
        /// The name of the current <see cref="MemberData"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The path to the current <see cref="MemberData"/>.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Does the specified left hand side <see cref="MemberData"/> equal the specified right hand side <see cref="MemberData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="MemberData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="MemberData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="MemberData"/> equals the specified right hand side <see cref="MemberData"/>; otherwise, false.</returns>
        public static bool operator ==(MemberData lhs, MemberData rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Path == rhs.Path;
        }

        /// <summary>
        /// Does the specified left hand side <see cref="MemberData"/> not equal the specified right hand side <see cref="MemberData"/>?
        /// </summary>
        /// <param name="lhs">Left hand side <see cref="MemberData"/>.</param>
        /// <param name="rhs">Right hand side <see cref="MemberData"/>.</param>
        /// <returns>True if the specified left hand side <see cref="MemberData"/> does not equal the specified right hand side <see cref="MemberData"/>; otherwise, false.</returns>
        public static bool operator !=(MemberData lhs, MemberData rhs)
        {
            return !(lhs == rhs);
        }

        ///<inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is MemberData memberData && this == memberData;
        }

        ///<inheritdoc/>
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        ///<inheritdoc/>
        public override string ToString()
        {
            return Path;
        }
    }
}