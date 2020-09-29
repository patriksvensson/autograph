using System.Diagnostics;
using Spectre.IO;

namespace Autograph
{
    [DebuggerDisplay("{Path.FullPath,nq}")]
    public sealed class Project
    {
        public string Name { get; set; }
        public FilePath Path { get; set; }
    }
}
