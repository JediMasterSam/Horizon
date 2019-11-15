using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Horizon.Diagnostics
{
    /// <summary>
    /// Represents the results of a boolean statement.
    /// </summary>
    internal sealed class Assertion
    {
        /// <summary>
        /// Did the current <see cref="Assertion"/> pass?
        /// </summary>
        private readonly bool _passed;

        /// <summary>
        /// Message that describes why the current <see cref="Assertion"/> passed or failed.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// The function type used for the current <see cref="Assertion"/>.
        /// </summary>
        private readonly string _type;

        /// <summary>
        /// The stack frame in which the current <see cref="Assertion"/> was made.
        /// </summary>
        private readonly StackFrame _stackFrame;

        /// <summary>
        /// Creates a new instance of <see cref="Assertion"/>.
        /// </summary>
        /// <param name="passed">Did the function return true?</param>
        /// <param name="message">Describes the pass/fail state.</param>
        /// <param name="type">Function type that generated the pass/fail state.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        private Assertion(bool passed, string message, string type, StackTraceTracker stackTraceTracker)
        {
            _passed = passed;
            _message = message;
            _type = type;
            _stackFrame = passed ? null : stackTraceTracker?.GetFrames().FirstOrDefault();
        }

        /// <summary>
        /// Implicitly converts the specified <see cref="Assertion"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="assertion"><see cref="Assertion"/> to be converted to a <see cref="bool"/>.</param>
        /// <returns>The specified <see cref="Assertion"/> as a <see cref="bool"/>.</returns>
        public static implicit operator bool(Assertion assertion)
        {
            return assertion._passed;
        }

        /// <summary>
        /// Does the specified expected value equal the specified actual value?
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>The results of the equality function in an <see cref="Assertion"/>.</returns>
        internal static Assertion AreEqual<T>(T expected, T actual, StackTraceTracker stackTraceTracker)
        {
            return AreEqual(expected, actual, nameof(AreEqual), stackTraceTracker);
        }

        /// <summary>
        /// Does the specified unexpected value not equal the specified actual value?
        /// </summary>
        /// <param name="unexpected">Unexpected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>The results of the inequality function in an <see cref="Assertion"/>.</returns>
        internal static Assertion AreNotEqual<T>(T unexpected, T actual, StackTraceTracker stackTraceTracker)
        {
            return AreNotEqual(unexpected, actual, nameof(AreNotEqual), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified condition true?
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <returns>The results of the equality function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsTrue(bool condition, StackTraceTracker stackTraceTracker)
        {
            return AreEqual(true, condition, nameof(IsTrue), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified condition false?
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <returns>The results of the equality function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsFalse(bool condition, StackTraceTracker stackTraceTracker)
        {
            return AreEqual(false, condition, nameof(IsFalse), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified value null?
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Reference type.</typeparam>
        /// <returns>The results of the equality function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsNull<T>(T value, StackTraceTracker stackTraceTracker) where T : class
        {
            return AreEqual(null, value, nameof(IsNull), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified value null?
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>The results of the equality function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsNull<T>(T? value, StackTraceTracker stackTraceTracker) where T : struct
        {
            return AreEqual(null, value, nameof(IsNull), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified value not null?
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Reference type.</typeparam>
        /// <returns>The results of the inequality function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsNotNull<T>(T value, StackTraceTracker stackTraceTracker) where T : class
        {
            return AreNotEqual(null, value, nameof(IsNotNull), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified value not null?
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>The results of the inequality function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsNotNull<T>(T? value, StackTraceTracker stackTraceTracker) where T : struct
        {
            return AreNotEqual(null, value, nameof(IsNotNull), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified left hand side value greater than the specified right hand side value?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Comparable type.</typeparam>
        /// <returns>The results of the comparison function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsGreaterThan<T>(T lhs, T rhs, StackTraceTracker stackTraceTracker) where T : IComparable
        {
            return Comparer.Default.Compare(lhs, rhs) > 0
                ? new Assertion(true, $"{lhs} is greater than {rhs}", nameof(IsGreaterThan), stackTraceTracker)
                : new Assertion(false, $"{lhs} is not greater than {rhs}", nameof(IsGreaterThan), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified left hand side value greater than or equal to the specified right hand side value?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Comparable type.</typeparam>
        /// <returns>The results of the comparison function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsGreaterThanOrEqualTo<T>(T lhs, T rhs, StackTraceTracker stackTraceTracker) where T : IComparable
        {
            return Comparer.Default.Compare(lhs, rhs) >= 0
                ? new Assertion(true, $"{ToString(lhs)} is greater than or equal to {ToString(rhs)}", nameof(IsGreaterThanOrEqualTo), stackTraceTracker)
                : new Assertion(false, $"{ToString(lhs)} is not greater than nor equal to {ToString(rhs)}", nameof(IsGreaterThanOrEqualTo), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified left hand side value less than the specified right hand side value?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Comparable type.</typeparam>
        /// <returns>The results of the comparison function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsLessThan<T>(T lhs, T rhs, StackTraceTracker stackTraceTracker) where T : IComparable
        {
            return Comparer.Default.Compare(lhs, rhs) < 0
                ? new Assertion(true, $"{ToString(lhs)} is less than {ToString(rhs)}", nameof(IsLessThan), stackTraceTracker)
                : new Assertion(false, $"{ToString(lhs)} is not less than {ToString(rhs)}", nameof(IsLessThan), stackTraceTracker);
        }

        /// <summary>
        /// Is the specified left hand side value less than or equal to the specified right hand side value?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Comparable type.</typeparam>
        /// <returns>The results of the comparison function in an <see cref="Assertion"/>.</returns>
        internal static Assertion IsLessThanOrEqualTo<T>(T lhs, T rhs, StackTraceTracker stackTraceTracker) where T : IComparable
        {
            return Comparer.Default.Compare(lhs, rhs) <= 0
                ? new Assertion(true, $"{ToString(lhs)} is less than or equal to {ToString(rhs)}", nameof(IsLessThanOrEqualTo), stackTraceTracker)
                : new Assertion(false, $"{ToString(lhs)} is not less than nor equal to {ToString(rhs)}", nameof(IsLessThanOrEqualTo), stackTraceTracker);
        }

        /// <summary>
        /// Does the specified collection contain the specified element?
        /// </summary>
        /// <param name="enumerable">Collection of elements.</param>
        /// <param name="element">Element.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>The results of the search function in an <see cref="Assertion"/>.</returns>
        internal static Assertion Contains<T>(IEnumerable<T> enumerable, T element, StackTraceTracker stackTraceTracker)
        {
            return enumerable.Contains(element)
                ? new Assertion(true, $"{ToString(element)} was found.", nameof(Contains), stackTraceTracker)
                : new Assertion(true, $"{ToString(element)} was not found.", nameof(Contains), stackTraceTracker);
        }

        /// <summary>
        /// Does the specified collection not contain the specified element?
        /// </summary>
        /// <param name="enumerable">Collection of elements.</param>
        /// <param name="element">Element.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>The results of the search function in an <see cref="Assertion"/>.</returns>
        internal static Assertion DoesNotContain<T>(IEnumerable<T> enumerable, T element, StackTraceTracker stackTraceTracker)
        {
            return enumerable.Contains(element)
                ? new Assertion(false, $"{ToString(element)} was found.", nameof(DoesNotContain), stackTraceTracker)
                : new Assertion(true, $"{ToString(element)} was not found.", nameof(DoesNotContain), stackTraceTracker);
        }

        /// <summary>
        /// Does the sequence of the specified expected collection match the sequence of the specified actual collection?
        /// </summary>
        /// <param name="expected">Expected sequence.</param>
        /// <param name="actual">Actual sequence.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>The results of the comparison functions as an <see cref="Assertion"/> collection.</returns>
        internal static IEnumerable<Assertion> SequenceEquals<T>(IEnumerable<T> expected, IEnumerable<T> actual, StackTraceTracker stackTraceTracker)
        {
            var expectedArray = expected.ToArray();
            var actualArray = actual.ToArray();

            if (expectedArray.Length != actualArray.Length)
            {
                yield return new Assertion(false, $"Expected {expectedArray.Length} elements but found {actualArray.Length} elements.", nameof(SequenceEquals), stackTraceTracker);
                yield break;
            }

            for (var index = 0; index < expectedArray.Length; index++)
            {
                yield return AreEqual(expectedArray[index], actualArray[index], nameof(SequenceEquals), stackTraceTracker);
            }
        }

        internal static Assertion IsEmpty<T>(IEnumerable<T> enumerable, StackTraceTracker stackTraceTracker)
        {
            return enumerable.Any()
                ? new Assertion(false, "Expected no elements but found elements.", nameof(IsEmpty), stackTraceTracker)
                : new Assertion(true, "Expected no elements and found no elements.", nameof(IsEmpty), stackTraceTracker);
        }
        
        internal static Assertion IsNotEmpty<T>(IEnumerable<T> enumerable, StackTraceTracker stackTraceTracker)
        {
            return enumerable.Any()
                ? new Assertion(true, "Expected elements and found elements.", nameof(IsNotEmpty), stackTraceTracker)
                : new Assertion(false, "Expected elements but found no elements.", nameof(IsNotEmpty), stackTraceTracker);
        }

        /// <summary>
        /// Does the specified expected value equal the specified actual value?
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <param name="type">Function type.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>The results of the equality function in an <see cref="Assertion"/>.</returns>
        private static Assertion AreEqual<T>(T expected, T actual, string type, StackTraceTracker stackTraceTracker)
        {
            return Equals(expected, actual)
                ? new Assertion(true, $"Expected {ToString(expected)} and got {ToString(actual)}.", type, stackTraceTracker)
                : new Assertion(false, $"Expected {ToString(expected)} but got {ToString(actual)}.", type, stackTraceTracker);
        }

        /// <summary>
        /// Does the specified unexpected value not equal the specified actual value?
        /// </summary>
        /// <param name="unexpected">Unexpected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <param name="type">Function type.</param>
        /// <param name="stackTraceTracker">Used to find where a failure occured.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>The results of the inequality function in an <see cref="Assertion"/>.</returns>
        private static Assertion AreNotEqual<T>(T unexpected, T actual, string type, StackTraceTracker stackTraceTracker)
        {
            return !Equals(unexpected, actual)
                ? new Assertion(true, $"Did not expect {ToString(unexpected)} and got {ToString(actual)}.", type, stackTraceTracker)
                : new Assertion(false, $"Did not expect {ToString(unexpected)} but got {ToString(actual)}.", type, stackTraceTracker);
        }

        /// <summary>
        /// Converts the specified value to <see cref="string"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>If the specified value is not null, <see cref="object.ToString()"/>; otherwise, 'null'.</returns>
        private static string ToString<T>(T value)
        {
            return value == null ? "null" : value.ToString();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (_passed)
            {
                return $" + {_type} passed. {_message}";
            }

            var fileNameAndLineNumber = GetFileNameAndLineNumber();

            return $" - {_type} failed. {_message}{(fileNameAndLineNumber != null ? $" {fileNameAndLineNumber}" : string.Empty)}";
        }

        /// <summary>
        /// Gets the file name and line number of the <see cref="StackFrame"/> of the current <see cref="Assertion"/>.
        /// </summary>
        /// <returns>The file name and line number formatted for console output.</returns>
        private string GetFileNameAndLineNumber()
        {
            if (_stackFrame == null) return null;

            var fileName = _stackFrame.GetFileName();
            var lineNumber = _stackFrame.GetFileLineNumber();

            if (string.IsNullOrEmpty(fileName) || lineNumber == 0) return null;

            return $"{fileName}: line {lineNumber}";
        }
    }
}