using System.Collections.Generic;
using System.IO;
using CommandLine;
using log4net;

namespace ConsoleAnalyzer
{
    class Program
    {
        static readonly ILog Logger = LogManager.GetLogger(typeof (Program));

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<DependecyOptions, AssemblyAnalyzerOptions>(args)
                .WithParsed<DependecyOptions>(options =>
                {
                    var assemblyProcessor = new AssemblyProcessor(options.Path, options.RecursionLimit);
                    ProcessAssemblyGraph(options, assemblyProcessor);
                    ProcessTypeGraph(options, assemblyProcessor);

                    Logger.Info("Done.");
                })
                .WithParsed<AssemblyAnalyzerOptions>(options =>
                {
                    var assemblyProcessor = new AssemblyProcessor(options.Path);
                    var assembly = assemblyProcessor.Assembly;

                    var info = new Dictionary<string, string>
                    {
                        { "Assembly name", assembly.FullyQualifiedName},
                        { "Architecture", assembly.Module.Architecture.ToString() },
                        { "Runtime version", assembly.Module.RuntimeVersion },
                        { "Has resources", assembly.Module.HasResources.ToString() }
                    };


                    foreach (var entry in info)
                    {
                        Logger.Info($"{entry.Key}: {entry.Value}");
                    }

                    Logger.Info("Done.");
                });
        }

        static void ProcessTypeGraph(DependecyOptions options, AssemblyProcessor assemblyProcessor)
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
