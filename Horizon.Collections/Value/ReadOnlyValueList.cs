using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Horizon.Collections
{
    /// <summary>
    /// Represents a readonly collection of <see cref="ValueElement"/>.
    /// </summary>
    public class ReadOnlyValueList : IReadOnlyList<ValueElement>
    {
        /// <summary>
        /// The elements of the current <see cref="ReadOnlyValueList"/>.
        /// </summary>
        private readonly ValueElement[] _elements;

        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlyValueList"/>.
        /// </summary>
        /// <param name="elements">Elements.</param>
        public ReadOnlyValueList(IEnumerable<ValueElement> elements)
        {
            _elements = elements.ToArray();

            Count = _elements.Length;
        }

        public ReadOnlyValueList(IEnumerable<object> values)
        {
            _elements = values.Select(value => new ValueElement(value)).ToArray();

            Count = _elements.Length;
        }

        /// <inheritdoc/>
        public int Count { get; }

        /// <inheritdoc/>
        public ValueElement this[int index] => _elements[index];

        /// <summary>
        /// Gets all elements in the current <see cref="ReadOnlyValueList"/> that are of type <see cref="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>Collection of <see cref="TValue"/>.</returns>
        public IEnumerable<TValue> Get<TValue>()
        {
            foreach (var element in _elements)
            {
                if (element.TryGetValue<TValue>(out var value))
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Does the current <see cref="ReadOnlyValueList"/> contain any elements of type <see cref="TValue"/>?
        /// </summary>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if the current <see cref="ReadOnlyValueList"/> contains an element of type <see cref="TValue"/>; otherwise, false.</returns>
        public bool Contains<TValue>()
        {
            return _elements.Any(element => element.Is<TValue>());
        }

        /// <summary>
        /// Gets the first element in the current <see cref="ReadOnlyValueList"/> that is of type <see cref="TValue"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if the current <see cref="ReadOnlyValueList"/> contains an element of type <see cref="TValue"/>; otherwise, false.</returns>
        public bool TryGet<TValue>(out TValue value)
        {
            foreach (var element in _elements)
            {
                if (element.TryGetValue<TValue>(out value))
                {
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public IEnumerator<ValueElement> GetEnumerator()
        {
            return _elements.OfType<ValueElement>().GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}