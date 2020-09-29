using System;

namespace Autograph
{
    public sealed class DirectedGraphEdge<T>
        where T : class
    {
        public T From { get; }
        public T To { get; }

        public DirectedGraphEdge(T from, T to)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
        }
    }
}
