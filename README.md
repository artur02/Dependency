# .NET dependency analyzer

## Build status

[![Build status](https://ci.appveyor.com/api/projects/status/be5nvg9whyxfucro?svg=true)](https://ci.appveyor.com/project/artur02/dependency)

## What is it?
.NET dependency analyzer is a simple tool to discover type and assembly reference 
dependency chains in compiled .NET assemblies.

### Command line options

    >ConsoleAnalyzer
    Console Analyzer 1.0.0.0
    Copyright c 2016 Artur Herczeg
    
    ERROR(S):
      No verb selected.
      
      dep        Dependency-related operations
      
      asm        Assembly analysis
      
      help       Display more information on a specific command.

      version    Display version information. 
      
## Dependency analysis
### Running the console application

    >ConsoleAnalyzer dep Analyzer.dll -a -r 1
    2016-01-15 17:21:23,338 [1] DEBUG ConsoleAnalyzer.AssemblyProcessor [(null)] - Parsing assembly: Analyzer.dll
    2016-01-15 17:21:23,381 [1] INFO  ConsoleAnalyzer.Program [(null)] - Generating assembly reference graph...
    2016-01-15 17:21:24,528 [1] DEBUG ConsoleAnalyzer.Program [(null)] - Writing assembly reference graph into file: graph_asm.graphml
    2016-01-15 17:21:24,530 [1] INFO  ConsoleAnalyzer.Program [(null)] - Done.

The graph representation (GraphML):  

    <?xml version="1.0" encoding="utf-8"?>
    <graphml xmlns="http://graphml.graphdrawing.org/xmlns">
    <key id="name" for="node" attr.name="name" attr.type="string" />
    <key id="fullyQualifiedName" for="node" attr.name="fullyQualifiedName" attr.type="string" />
    <graph id="G" edgedefault="directed" parse.nodes="40" parse.edges="207" parse.order="nodesfirst" parse.nodeids="free" parse.edgeids="free">
        <node id="C:\Source\Dependency\DependencyAnalyzer\ConsoleAnalyzer\bin\Debug\Analyzer.dll">
            <data key="name">Analyzer</data>
            <data key="fullyQualifiedName">C:\Source\Dependency\DependencyAnalyzer\ConsoleAnalyzer\bin\Debug\Analyzer.dll</data>
        </node>
        <node id="C:\Source\Dependency\DependencyAnalyzer\ConsoleAnalyzer\bin\Debug\Mono.Cecil.dll">
            <data key="name">Mono.Cecil</data>
            <data key="fullyQualifiedName">C:\Source\Dependency\DependencyAnalyzer\ConsoleAnalyzer\bin\Debug\Mono.Cecil.dll</data>
        </node>
        <node id="C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll">
            <data key="name">CommonLanguageRuntimeLibrary</data>
            <data key="fullyQualifiedName">C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll</data>
        </node>
    [...]


### Visalizing the dependency graph

The standard GraphML file can be loaded into numerous graph visualization and manipulation tools.
I can recommend the followings:
* [Cytoscape - Network Data Integration, Analysis, and Visualization in a Box](http://www.cytoscape.org/)
* [Gephi - The Open Graph Viz Platform](https://gephi.org/)

The example below is rendered in Gephi:

![Dependency visualisation example](/Documentation/DirectAssemblyDependencyExample.PNG)