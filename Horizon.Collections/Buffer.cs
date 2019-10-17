using System;
using System.Collections;
using System.Collections.Generic;

namespace Horizon.Collections
{
    public sealed class Buffer<TValue> : IEnumerable<BufferElement<TValue>> where TValue : IDisposable, new()
    {
        private readonly object _bufferLock;

        public Buffer(int capacity)
        {
            _bufferLock = new object();

            Count = 0;
            Capacity = capacity;
            Elements = new BufferElement<TValue>[capacity];

            for (var index = 0; index < Elements.Length; index++)
            {
                Elements[index] = new BufferElement<TValue>(this);
            }
        }

        public int Count { get; private set; }

        public int Capacity { get; private set; }

        private int ActiveCount { get; set; }

        private BufferElement<TValue>[] Elements { get; set; }

        public BufferElement<TValue> Activate()
        {
            lock (_bufferLock)
            {
                if (ActiveCount == Capacity)
                {
                    if (ActiveCount == Count)
                    {
                        var elements = new BufferElement<TValue>[Capacity *= 2];

                        Array.Copy(Elements, elements, Count);

                        for (var index = Count; index < Capacity; index++)
                        {
                            elements[index] = new BufferElement<TValue>(this);
                        }

                        Elements = elements;
                    }
                    else
                    {
                        for (var index = ActiveCount - 1; index >= 0; index--)
                        {
                            if (!Elements[index].Active)
                            {
                                Swap(index, --ActiveCount);
                            }
                        }
                    }
                }

                var bufferElement = Elements[Count++];

                if (!bufferElement.Active)
                {
                    bufferElement.Active = true;
                    ActiveCount++;
                }

                return bufferElement;
            }
        }

        public void Clear()
        {
            lock (_bufferLock)
            {
                Count = 0;
            }
        }

        public IEnumerator<BufferElement<TValue>> GetEnumerator()
        {
            lock (_bufferLock)
            {
                for (var index = ActiveCount - 1; index >= 0; index--)
                {
                    var bufferElement = Elements[index];

                    if (!bufferElement.Active)
                    {
                        Swap(index, --ActiveCount);
                    }
                    else
                    {
                        yield return bufferElement;
                    }
                }
            }
        }

        internal void Deactivate(BufferElement<TValue> bufferElement)
        {
            lock (_bufferLock)
            {
                if (!bufferElement.Active) return;

                bufferElement.Active = false;
                bufferElement.Dispose();
                Count--;
            }
        }

        private void Swap(int lhs, int rhs)
        {
            var bufferElement = Elements[lhs];

            Elements[lhs] = Elements[rhs];
            Elements[rhs] = bufferElement;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}