using System;

namespace Horizon.Collections
{
    public sealed class BufferElement<TValue> where TValue : IDisposable, new()
    {
        private readonly Buffer<TValue> _buffer;

        internal BufferElement(Buffer<TValue> buffer)
        {
            _buffer = buffer;

            Active = false;
            Value = new TValue();
        }

        public TValue Value { get; }

        internal bool Active { get; set; }

        public void Deactivate()
        {
            _buffer.Deactivate(this);
        }

        internal void Dispose()
        {
            Value.Dispose();
        }
    }
}