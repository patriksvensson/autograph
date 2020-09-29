using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Spectre.IO;

namespace Autograph
{
    public sealed class ProjectParser
    {
        private readonly IFileSystem _fileSystem;

        public ProjectParser(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public IEnumerable<FilePath> GetReferences(Project project)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            var root = project.Path.GetDirectory();

            var file = _fileSystem.GetFile(project.Path);
            using var stream = file.OpenRead();

            var document = XDocument.Load(stream);

            // Old project format?
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            var references = document
                ?.Element(msbuild + "Project")
                ?.Elements(msbuild + "ItemGroup")
                ?.Elements(msbuild + "ProjectReference");

            if (references == null)
            {
                // New project format?
                references = document
                    ?.Element("Project")
                    ?.Elements("ItemGroup")
                    ?.Elements("ProjectReference");

                if (references == null)
                {
                    return Enumerable.Empty<FilePath>();
                }
            }

            var result = new List<FilePath>();
            foreach (var reference in references)
            {
                var include = reference.Attribute("Include")?.Value;
                if (!string.IsNullOrWhiteSpace(include))
                {
                    var includePath = new FilePath(include);
                    result.Add(includePath.MakeAbsolute(root));
                }
            }

            return result;
        }
    }
}
