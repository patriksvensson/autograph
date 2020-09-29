using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.IO;

namespace Autograph
{
    public sealed class GraphBuilder
    {
        private readonly IFileSystem _fileSystem;
        private readonly ProjectParser _parser;

        public GraphBuilder(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _parser = new ProjectParser(_fileSystem);
        }

        public DirectedGraph<Project> Build(IEnumerable<Project> projects)
        {
            if (projects is null)
            {
                throw new ArgumentNullException(nameof(projects));
            }

            var comparer = new ProjectComparer();
            var graph = new DirectedGraph<Project>(comparer);
            var visited = new HashSet<Project>(comparer);

            var stack = new Stack<Project>();
            foreach (var project in projects)
            {
                stack.Push(project);
            }

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (visited.Contains(current))
                {
                    continue;
                }

                visited.Add(current);
                graph.Add(current);

                foreach (var reference in GetProjectReferences(current))
                {
                    stack.Push(reference);
                    graph.Connect(current, reference);
                }
            }

            return graph;
        }

        private IEnumerable<Project> GetProjectReferences(Project project)
        {
            var references = _parser.GetReferences(project);
            return references.Select(reference => new Project
            {
                Name = reference.GetFilename().FullPath,
                Path = reference,
            });
        }
    }
}
