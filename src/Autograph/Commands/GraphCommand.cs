using System;
using System.Linq;
using Spectre.Cli;
using Spectre.IO;

namespace Autograph
{
    public sealed class GraphCommand : Command<GraphCommandSettings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly IEnvironment _environment;

        public GraphCommand(IFileSystem fileSystem, IEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public override ValidationResult Validate(CommandContext context, GraphCommandSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            foreach (var project in settings.Projects)
            {
                if (!_fileSystem.File.Exists(project))
                {
                    var message = $"The project '{project.FullPath}' does not exist";
                    return ValidationResult.Error(message);
                }
            }

            return base.Validate(context, settings);
        }

        public override int Execute(CommandContext context, GraphCommandSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // Build the graph
            var builder = new GraphBuilder(_fileSystem);
            var graph = builder.Build(settings.Projects.Select(project =>
            {
                var path = project.MakeAbsolute(_environment);
                return new Project
                {
                    Name = path.GetFilename().FullPath,
                    Path = path,
                };
            }));

            // Write the graph
            var writer = new GraphWriter(_environment);
            writer.Write(settings.Output, graph);

            return 0;
        }
    }
}
