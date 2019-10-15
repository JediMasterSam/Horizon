using System;
using System.Collections.Generic;
using System.Linq;

namespace Horizon.Diagnostics
{
    /// <summary>
    /// Represents a collection of <see cref="Assertion"/>.
    /// </summary>
    internal sealed class AssertionLog
    {
        /// <summary>
        /// Assertions in the current <see cref="AssertionLog"/>.
        /// </summary>
        private readonly List<Assertion> _assertions;

        /// <summary>
        /// Should the output only show failed assertions?
        /// </summary>
        private readonly bool _showOnlyFailures;

        /// <summary>
        /// The total number of assertions logged in the current <see cref="AssertionLog"/>.
        /// </summary>
        private int _count;

        /// <summary>
        /// The total number of failed assertions in the current <see cref="AssertionLog"/>.
        /// </summary>
        private int _failures;

        private Exception _exception;

        /// <summary>
        /// Creates a new instance of <see cref="AssertionLog"/>.
        /// </summary>
        /// <param name="showOnlyFailures">Should the output only show failed assertions?</param>
        internal AssertionLog(bool showOnlyFailures)
        {
            _assertions = new List<Assertion>();
            _showOnlyFailures = showOnlyFailures;
            _count = _failures = 0;
        }

        /// <summary>
        /// Implicitly converts the specified <see cref="AssertionLog"/> in to a <see cref="bool"/>.
        /// </summary>
        /// <param name="assertionLog">Assertion log.</param>
        /// <returns>True if the specified <see cref="AssertionLog"/> contains no failed assertions; otherwise, false.</returns>
        public static implicit operator bool(AssertionLog assertionLog)
        {
            return assertionLog._failures == 0 && assertionLog._exception == null;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Assertions: {_count}, Passed: {_count - _failures}, Failed: {_failures}\n{string.Join("\n", _assertions.Select(assertion => assertion.ToString()))}{(_exception != null ? $"\n{_exception}" : string.Empty)}";
        }

        /// <summary>
        /// Adds the specified <see cref="Assertion"/> to the current <see cref="AssertionLog"/>.
        /// </summary>
        /// <param name="assertion">Assertion.</param>
        /// <returns>True if the specified <see cref="Assertion"/> is true; otherwise, false.</returns>
        internal bool Add(Assertion assertion)
        {
            if (!assertion)
            {
                _assertions.Add(assertion);
                _count++;
                _failures++;
            }
            else if (!_showOnlyFailures)
            {
                _assertions.Add(assertion);
                _count++;
            }

            return assertion;
        }

        /// <summary>
        /// Adds the specified <see cref="Exception"/> to the current <see cref="AssertionLog"/>.
        /// </summary>
        /// <param name="exception">Exception.</param>
        internal void Add(Exception exception)
        {
            _exception = exception;
        }
    }
}