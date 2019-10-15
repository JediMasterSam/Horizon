using System;

namespace Horizon.Diagnostics
{
    /// <summary>
    /// Represents a failed test.
    /// </summary>
    internal sealed class TestFailedException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="TestFailedException"/>.
        /// </summary>
        /// <param name="message">Message.</param>
        internal TestFailedException(string message) : base(message)
        {
            StackTrace = string.Empty;
        }

        /// <inheritdoc/>
        public override string StackTrace { get; }
    }
}