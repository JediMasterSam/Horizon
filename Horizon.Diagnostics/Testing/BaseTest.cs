using System;
using System.Collections.Generic;
using System.Linq;

namespace Horizon.Diagnostics
{
    /// <summary>
    /// Represents the base of a unit test.
    /// </summary>
    public abstract class BaseTest
    {
        /// <summary>
        /// <see cref="AssertionLog"/>s for the current <see cref="BaseTest"/>.
        /// </summary>
        private readonly List<AssertionLog> _assertionLogs;

        /// <summary>
        /// Used to find where failures occured in the current <see cref="BaseTest"/>.
        /// </summary>
        private readonly StackTraceTracker _stackTraceTracker;

        /// <summary>
        /// The <see cref="AssertionLog"/> log for the current <see cref="BaseTest"/>.
        /// </summary>
        private AssertionLog _assertionLog;

        /// <summary>
        /// The <see cref="Type"/> of <see cref="Exception"/> expected.
        /// </summary>
        private Type _exceptionType;

        /// <summary>
        /// Has an <see cref="Exception"/> been caught?
        /// </summary>
        private bool _caughtException;

        /// <summary>
        /// Creates a new instance of <see cref="BaseTest"/>.
        /// </summary>
        protected BaseTest()
        {
            _assertionLogs = new List<AssertionLog>();
            _stackTraceTracker = new StackTraceTracker(GetType());

            ShowOnlyFailures = false;
        }

        /// <summary>
        /// Should the output only show failed assertions?
        /// </summary>
        protected bool ShowOnlyFailures { get; set; }

        public override string ToString()
        {
            return $"\n{(_assertionLogs.Count == 1 ? _assertionLog.ToString() : string.Join("\n\n", _assertionLogs.Select((assertionLog, index) => $"Case {index + 1}:\n{assertionLog.ToString()}")))}";
        }

        /// <summary>
        /// Does the specified expected value equal the specified actual value?
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified expected value equals the specified actual value; otherwise, false.</returns>
        protected bool AreEqual<T>(T expected, T actual)
        {
            return _assertionLog.Add(Assertion.AreEqual(expected, actual, _stackTraceTracker));
        }

        /// <summary>
        /// Does the specified unexpected value equal the specified actual value?
        /// </summary>
        /// <param name="unexpected">Unexpected value.</param>
        /// <param name="actual">Actual value.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified unexpected value dose not equal the specified actual value; otherwise, false.</returns>
        protected bool AreNotEqual<T>(T unexpected, T actual)
        {
            return _assertionLog.Add(Assertion.AreNotEqual(unexpected, actual, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified condition true?
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <returns>True if the specified condition is true; otherwise, false.</returns>
        protected bool IsTrue(bool condition)
        {
            return _assertionLog.Add(Assertion.IsTrue(condition, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified condition false?
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <returns>True if the specified condition is false; otherwise, false.</returns>
        protected bool IsFalse(bool condition)
        {
            return _assertionLog.Add(Assertion.IsFalse(condition, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified value null?
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Reference type.</typeparam>
        /// <returns>True if the specified value is null; otherwise, false.</returns>
        protected bool IsNull<T>(T value) where T : class
        {
            return _assertionLog.Add(Assertion.IsNull(value, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified value null?
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>True if the specified value is null; otherwise, false.</returns>
        protected bool IsNull<T>(T? value) where T : struct
        {
            return _assertionLog.Add(Assertion.IsNull(value, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified value not null?
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Reference type.</typeparam>
        /// <returns>True if the specified value is not null; otherwise, false.</returns>
        protected bool IsNotNull<T>(T value) where T : class
        {
            return _assertionLog.Add(Assertion.IsNotNull(value, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified value not null?
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>True if the specified value is not null; otherwise, false.</returns>
        protected bool IsNotNull<T>(T? value) where T : struct
        {
            return _assertionLog.Add(Assertion.IsNotNull(value, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified left hand side value greater than the specified right hand side value?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified left hand side is greater than the specified right hand side; otherwise, false.</returns>
        protected bool IsGreaterThan<T>(T lhs, T rhs) where T : IComparable
        {
            return _assertionLog.Add(Assertion.IsGreaterThan(lhs, rhs, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified left hand side value greater than or equal to the specified right hand side value?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified left hand side is greater than or equal to the specified right hand side; otherwise, false.</returns>
        protected bool IsGreaterThanOrEqualTo<T>(T lhs, T rhs) where T : IComparable
        {
            return _assertionLog.Add(Assertion.IsGreaterThanOrEqualTo(lhs, rhs, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified left hand side value less than the specified right hand side value?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified left hand side is less than the specified right hand side; otherwise, false.</returns>
        protected bool IsLessThan<T>(T lhs, T rhs) where T : IComparable
        {
            return _assertionLog.Add(Assertion.IsLessThan(lhs, rhs, _stackTraceTracker));
        }

        /// <summary>
        /// Is the specified left hand side value less than or equal to the specified right hand side value?
        /// </summary>
        /// <param name="lhs">Left hand side.</param>
        /// <param name="rhs">Right hand side.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified left hand side is less than or equal to the specified right hand side; otherwise, false.</returns>
        protected bool IsLessThanOrEqualTo<T>(T lhs, T rhs) where T : IComparable
        {
            return _assertionLog.Add(Assertion.IsLessThanOrEqualTo(lhs, rhs, _stackTraceTracker));
        }

        /// <summary>
        /// Does the specified collection contain the specified element?
        /// </summary>
        /// <param name="enumerable">Collection of elements.</param>
        /// <param name="element">Element.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified collection contains the specified element; otherwise, false.</returns>
        protected bool Contains<T>(IEnumerable<T> enumerable, T element)
        {
            return _assertionLog.Add(Assertion.Contains(enumerable, element, _stackTraceTracker));
        }

        /// <summary>
        /// Does the specified collection not contain the specified element?
        /// </summary>
        /// <param name="enumerable">Collection of elements.</param>
        /// <param name="element">Element.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified collection does not contain the specified element; otherwise, false.</returns>
        protected bool DoesNotContain<T>(IEnumerable<T> enumerable, T element)
        {
            return _assertionLog.Add(Assertion.DoesNotContain(enumerable, element, _stackTraceTracker));
        }

        /// <summary>
        /// Does the sequence of the specified expected collection match the sequence of the specified actual collection?
        /// </summary>
        /// <param name="expected">Expected sequence.</param>
        /// <param name="actual">Actual sequence.</param>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>True if the specified expected sequence equals the specified actual sequence; otherwise, false.</returns>
        protected bool SequenceEquals<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            return Assertion.SequenceEquals(expected, actual, _stackTraceTracker).Aggregate(true, (passed, assertion) => passed && _assertionLog.Add(assertion));
        }

        /// <summary>
        /// Sets the expected <see cref="Exception"/>.
        /// </summary>
        /// <typeparam name="T">Exception type.</typeparam>
        protected void Expect<T>() where T : Exception
        {
            _exceptionType = typeof(T);
            _caughtException = false;
        }

        /// <summary>
        /// Sets the expected <see cref="Exception"/>.
        /// </summary>
        /// <param name="exceptionType">Exception type.</param>
        protected void Expect(Type exceptionType)
        {
            _exceptionType = exceptionType;
            _caughtException = false;
        }

        /// <summary>
        /// Runs the specified test.
        /// </summary>
        /// <param name="test">Test.</param>
        protected void Run(Action test)
        {
            Initialize();

            try
            {
                test.Invoke();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }

            CheckForException();
            Cleanup();
        }

        /// <summary>
        /// Runes the specified test for each specified test case.
        /// </summary>
        /// <param name="test">Test.</param>
        /// <param name="testCases">Test cases.</param>
        /// <typeparam name="T">Type.</typeparam>
        protected void Run<T>(Action<T> test, params T[] testCases)
        {
            foreach (var testCase in testCases)
            {
                Initialize();

                try
                {
                    test.Invoke(testCase);
                }
                catch (Exception exception)
                {
                    HandleException(exception);
                }

                CheckForException();
            }

            Cleanup();
        }


        /// <summary>
        /// Initializes values before a test can run.
        /// </summary>
        private void Initialize()
        {
            _assertionLog = new AssertionLog(ShowOnlyFailures);
            _assertionLogs.Add(_assertionLog);
            _caughtException = false;
        }

        /// <summary>
        /// Outputs results from test.
        /// </summary>
        /// <exception cref="TestFailedException">The test failed.</exception>
        private void Cleanup()
        {
            if (_assertionLogs.Any(assertionLog => !assertionLog))
            {
                throw new TestFailedException(ToString());
            }

            Console.WriteLine(ToString());
        }

        /// <summary>
        /// Handles a <see cref="Exception"/> thrown by a test.
        /// </summary>
        /// <param name="exception">Exception.</param>
        private void HandleException(Exception exception)
        {
            if (_exceptionType == null || !_assertionLog.Add(Assertion.AreEqual(_exceptionType, exception.GetType(), null)))
            {
                _assertionLog.Add(exception);
            }

            _caughtException = true;
        }

        /// <summary>
        /// Verifies if there was an expected <see cref="Exception"/> that it has been handled.
        /// </summary>
        private void CheckForException()
        {
            if (_exceptionType != null && !_caughtException)
            {
                _assertionLog.Add(Assertion.AreEqual(_exceptionType, null, null));
            }
        }
    }
}