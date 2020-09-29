using System;
using System.Collections.Generic;
using System.Linq;

namespace Autograph
{
    public sealed class DirectedGraph<T>
        where T : class
    {
        private readonly HashSet<T> _nodes;
        private readonly List<DirectedGraphEdge<T>> _edges;
        private readonly IEqualityComparer<T> _comparer;

        public IReadOnlyCollection<T> Nodes => _nodes;
        public IReadOnlyCollection<DirectedGraphEdge<T>> Edges => _edges;

        public DirectedGraph(IEqualityComparer<T> comparer = null)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
            _nodes = new HashSet<T>(_comparer);
            _edges = new List<DirectedGraphEdge<T>>();
        }

        public void Add(T node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            _nodes.Add(node);
        }

        public void Connect(T from, T to)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (_comparer.Equals(from, to))
            {
                throw new InvalidOperationException("Reflexive edges in graph are not allowed.");
            }

            if (_edges.Any(x => _comparer.Equals(x.From, to) && _comparer.Equals(x.To, from)))
            {
                throw new InvalidOperationException("Unidirectional edges in graph are not allowed.");
            }

            if (_edges.Any(x => _comparer.Equals(x.From, from) && _comparer.Equals(x.To, to)))
            {
                return;
            }

            if (_nodes.Contains(from))
            {
                _nodes.Add(from);
            }

            if (_nodes.Contains(to))
            {
                _nodes.Add(to);
            }

            _edges.Add(new DirectedGraphEdge<T>(from, to));
        }
    }
}
