using System;
using CommandLine;

namespace ConsoleAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(options =>
                {
                    var assemblyProcessor = new AssemblyProcessor(options.Path);
                    assemblyProcessor.Process();
                });
        }
    }
}
