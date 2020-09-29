using System.ComponentModel;
using Spectre.Cli;
using Spectre.IO;

namespace Autograph
{
    public sealed class GraphCommandSettings : CommandSettings
    {
        [CommandArgument(0, "<PROJECTS>")]
        [TypeConverter(typeof(FilePathConverter))]
        [Description("The project(s) to build a graph from")]
        public FilePath[] Projects { get; set; }

        [CommandOption("--output <OUTPUT>")]
        [TypeConverter(typeof(FilePathConverter))]
        [Description("The filename of the resulting graph")]
        public FilePath Output { get; set; }
    }
}
