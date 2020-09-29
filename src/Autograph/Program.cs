using Autofac;
using Spectre.Cli;
using Spectre.IO;

namespace Autograph
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var registrar = new AutofacTypeRegistrar(BuildContainer());
            var app = new CommandApp<GraphCommand>(registrar);
            app.Configure(config =>
            {
                config.SetApplicationName("dotnet autograph");

                config.ValidateExamples();
                config.AddExample(new[] { "foo.csproj", });
                config.AddExample(new[] { "foo.csproj", "bar.csproj", "--output", "graph.dot" });
            });

            return app.Run(args);
        }

        private static ContainerBuilder BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<Environment>().As<IEnvironment>().SingleInstance();
            return builder;
        }
    }
}
