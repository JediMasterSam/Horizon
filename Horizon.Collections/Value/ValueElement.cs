namespace Horizon.Collections
{
    /// <summary>
    /// Represents a value.
    /// </summary>
    public sealed class ValueElement
    {
        /// <summary>
        /// The value of the current <see cref="ValueElement"/>.
        /// </summary>
        private readonly object _value;

        /// <summary>
        /// Creates a new instance of <see cref="ValueElement"/>.
        /// </summary>
        /// <param name="value"></param>
        public ValueElement(object value)
        {
            _value = value;
        }

        /// <summary>
        /// Is the current value of type <see cref="TValue"/>?
        /// </summary>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if the current type is of type <see cref="TValue"/>; otherwise, false.</returns>
        public bool Is<TValue>()
        {
            return _value is TValue;
        }

        /// <summary>
        /// Gets the current value as type <see cref="TValue"/>.
        /// </summary>
        /// <param name="value">Value as <see cref="TValue"/>.</param>
        /// <typeparam name="TValue">Type.</typeparam>
        /// <returns>True if the current type is of type <see cref="TValue"/>; otherwise, false.</returns>
        public bool TryGetValue<TValue>(out TValue value)
        {
            if (_value is TValue tempValue)
            {
                value = tempValue;
                return true;
            }

            value = default;
            return false;
        }
    }
}