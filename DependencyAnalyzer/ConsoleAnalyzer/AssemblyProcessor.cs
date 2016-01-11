using System.IO;
using System.Linq;
using Analyzer;
using Analyzer.ReturnTypes;
using Grapher;
using log4net;

namespace ConsoleAnalyzer
{
    public class AssemblyProcessor
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (AssemblyProcessor));

        public AssemblyProcessor(string assemblyPath)
        {
            logger.Debug($"Parsing assembly: {assemblyPath}");

            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException("Cannot find assembly.", assemblyPath);
            }

            Assembly = new Assembly(assemblyPath, new ComponentCache());
        }

        public Assembly Assembly { get; private set; }

        public string GetReferencedAssembliesGraph()
        {
            var references = Assembly.GetReferences();
            var g = new AsmReferenceGraphConverter(references);
            var graph = g.Convert();

            return graph;
        }

        public string GetReferencedTypesGraph()
        {
            var types = Assembly.GetTypes();

            var referencedTypes = new TypeReferenceCountDict();
            var typereferences = types.Select(x => x.GetReferencedTypes()).ToList();
            foreach (var typeref in typereferences)
            {
                foreach (var type in typeref)
                {
                    if (!referencedTypes.ContainsKey(type.Key))
                    {
                        referencedTypes.Add(type.Key, type.Key.GetReferencedTypes());
                    }
                }
            }
            var g2 = new TypeReferenceCountDictGraphConverter(referencedTypes);
            var graph2 = g2.Convert();

            return graph2;
        }
    }
}