using CommandLine;

namespace ConsoleAnalyzer
{
    class CommandLineOptions
    {
        [Option('p', "path", Required = true, HelpText = "Path to the entry point (.dll, .exe)")]
        public string Path { get; set; }
    }
}
