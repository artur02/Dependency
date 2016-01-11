using System.IO;
using CommandLine;
using log4net;

namespace ConsoleAnalyzer
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (Program));

        static void Main(string[] args)
        {
            //BasicConfigurator.Configure();

            var result = Parser.Default.ParseArguments<DependecyOptions>(args)
                .WithParsed<DependecyOptions>(options =>
                {
                    var assemblyProcessor = new AssemblyProcessor(options.Path);
                    ProcessAssemblyGraph(options, assemblyProcessor);
                    ProcessTypeGraph(options, assemblyProcessor);

                    Logger.Info("Done.");
                });
        }

        private static void ProcessTypeGraph(DependecyOptions options, AssemblyProcessor assemblyProcessor)
        {
            if (options.GenerateTypeGraph)
            {
                Logger.Info("Generating type reference graph...");
                var graph = assemblyProcessor.GetReferencedTypesGraph();
                if (!string.IsNullOrWhiteSpace(options.TypeGraphFileName))
                {
                    Logger.Debug($"Writing type reference graph into file: {options.AssemblyGraphFileName}");
                    File.WriteAllText(options.AssemblyGraphFileName, graph);
                }
                else
                {
                    Logger.Info(graph);
                }
            }
        }

        private static void ProcessAssemblyGraph(DependecyOptions options, AssemblyProcessor assemblyProcessor)
        {
            if (options.GenerateAssemblyGraph)
            {
                Logger.Info("Generating assembly reference graph...");

                var graph = assemblyProcessor.GetReferencedAssembliesGraph();
                if (!string.IsNullOrWhiteSpace(options.AssemblyGraphFileName))
                {
                    Logger.Debug($"Writing assembly reference graph into file: {options.AssemblyGraphFileName}");
                    File.WriteAllText(options.AssemblyGraphFileName, graph);
                }
                else
                {
                    Logger.Info(graph);
                }
            }
        }
    }
}
