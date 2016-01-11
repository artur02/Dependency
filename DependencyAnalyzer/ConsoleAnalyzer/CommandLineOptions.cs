﻿using CommandLine;

namespace ConsoleAnalyzer
{
    [Verb("dep", HelpText = "Dependency-related operations")]
    public class DependecyOptions
    {
        [Value(0, Required = true, HelpText = "Path to the entry point (.dll, .exe)")]
        public string Path { get; set; }

        [Option('a', "assembly", Default = false, HelpText = "Gets the assemblies referenced by the root assembly")]
        public bool GenerateAssemblyGraph { get; set; }

        [Option("asmgraphfile", Default = "graph_asm.graphml", HelpText = "File name of the generated assembly graph")]
        public string AssemblyGraphFileName { get; set; }

        [Option('t', "type", Default = false, HelpText = "Gets the types referenced by the root assembly")]
        public bool GenerateTypeGraph { get; set; }

        [Option("typegraphfile", Default = "graph_type.graphml", HelpText = "File name of the generated type graph")]
        public string TypeGraphFileName { get; set; }
    }
}
