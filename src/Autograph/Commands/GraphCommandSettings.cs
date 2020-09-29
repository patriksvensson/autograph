using System.ComponentModel;
using Spectre.Cli;
using Spectre.IO;

namespace Autograph
{
    public sealed class GraphCommandSettings : CommandSettings
    {
        [CommandArgument(0, "<PROJECTS>")]
        [TypeConverter(typeof(FilePathConverter))]
        public FilePath[] Projects { get; set; }

        [CommandOption("--output <OUTPUT>")]
        [TypeConverter(typeof(FilePathConverter))]
        public FilePath Output { get; set; }
    }
}
