using System;
using GiGraph.Dot.Entities.Graphs;
using GiGraph.Dot.Extensions;
using Spectre.IO;

namespace Autograph
{
    public sealed class GraphWriter
    {
        private readonly IEnvironment _environment;

        public GraphWriter(IEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public void Write(FilePath path, DirectedGraph<Project> graph)
        {
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            var dot = new DotGraph(isDirected: true);
            foreach (var edge in graph.Edges)
            {
                dot.Edges.Add(edge.From.Name, edge.To.Name);
            }

            path ??= new FilePath("graph.dot").MakeAbsolute(_environment);
            dot.SaveToFile(path.FullPath);
        }
    }
}
