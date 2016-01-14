using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Analyzer;
using Analyzer.ReturnTypes;
using Grapher.GraphConverter;
using log4net;

namespace ConsoleAnalyzer
{
    public class AssemblyProcessor
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (AssemblyProcessor));

        /// <exception cref="FileNotFoundException">Cannot find assembly.</exception>
        public AssemblyProcessor(string assemblyPath)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(assemblyPath));

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
            var assemblyReferenceGraph = new AsmReferenceGraphConverter(references);
            var graphMarkup = assemblyReferenceGraph.Convert();

            return graphMarkup;
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
            var typeReferenceGraph = new TypeReferenceCountDictGraphConverter(referencedTypes);
            var graphMarkup = typeReferenceGraph.Convert();

            return graphMarkup;
        }
    }
}