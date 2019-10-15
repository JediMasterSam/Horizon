using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Horizon.Diagnostics
{
    /// <summary>
    /// Represents a filtered stack trace. 
    /// </summary>
    public sealed class StackTraceTracker
    {
        /// <summary>
        /// The declaring type that the current <see cref="StackTraceTracker"/> will search for.
        /// </summary>
        private readonly string _declaringType;

        /// <summary>
        /// Creates a new instance of <see cref="StackTraceTracker"/>.
        /// </summary>
        /// <param name="type">The declaring type that will be use as the stack trace filter.</param>
        /// <exception cref="ArgumentNullException">The specified type cannot be null.</exception>
        public StackTraceTracker(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException($"{nameof(type)} cannot be null.");
            }

            _declaringType = type.FullName;
        }

        /// <summary>
        /// Gets all every <see cref="StackFrame"/> in the context's <see cref="StackTrace"/> that matches the declaring type of the current <see cref="StackTraceTracker"/>.
        /// </summary>
        /// <returns>A collection of <see cref="StackFrame"/> where the declaring type equals the declaring type of the current <see cref="StackTraceTracker"/>.</returns>
        public IEnumerable<StackFrame> GetFrames()
        {
            var stackFrames = new StackTrace(true).GetFrames();

            if (stackFrames == null || stackFrames.Length == 0) yield break;

            foreach (var stackFrame in stackFrames)
            {
                var declaringType = stackFrame?.GetMethod()?.DeclaringType?.FullName;

                if (string.IsNullOrEmpty(declaringType)) continue;

                var index = declaringType.LastIndexOf('+');

                if (index > 0)
                {
                    declaringType = declaringType.Substring(0, index);
                }

                if (_declaringType == declaringType)
                {
                    yield return stackFrame;
                }
            }
        }
    }
}