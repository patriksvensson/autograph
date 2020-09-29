using System;
using System.IO;
using System.Text;
using Spectre.IO;

namespace Autograph
{
    public sealed class GraphWriter
    {
        private readonly IFileSystem _fileSystem;
        private readonly IEnvironment _environment;

        public GraphWriter(IFileSystem fileSystem, IEnvironment environment)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public void Write(FilePath path, DirectedGraph<Project> graph)
        {
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            path ??= new FilePath("graph.dot").MakeAbsolute(_environment);

            using (var stream = _fileSystem.File.OpenWrite(path))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(BuildDotGraph(graph));
            }
        }

        private static StringBuilder BuildDotGraph(DirectedGraph<Project> graph)
        {
            var output = new StringBuilder();
            output.AppendLine("digraph sample {");

            foreach (var edge in graph.Edges)
            {
                output.Append('\"').Append(edge.From.Name).Append("\" -> \"").Append(edge.To.Name).AppendLine("\"");
            }

            output.AppendLine("}");
            return output;
        }
    }
}
