using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Analyzer;
using Analyzer.ReturnTypes;
using Grapher.GraphMarkup;
using QuickGraph;

namespace Grapher.GraphConverter
{
    /// <summary>
    /// Assembly reference to graph converter
    /// </summary>
    public class AsmReferenceGraphConverter : IGraphConverter
    {
        readonly AsmReference asmReference;
        readonly IGraphMarkup<IAssembly> markup;

        public AsmReferenceGraphConverter(AsmReference asmReference, IGraphMarkup<IAssembly> markup = null)
        {
            Contract.Requires<ArgumentNullException>(asmReference != null);

            this.asmReference = asmReference;
            this.markup = markup ?? new GraphML<IAssembly>();
        }

        /// <summary>
        /// Converts an assembly reference to GraphML representation
        /// </summary>
        /// <returns>GraphML markup</returns>
        public string Convert()
        {
            var graph = CreateGraph();
            var result = markup.Serialize(graph, v => v.FullyQualifiedName);

            return result;
        }

        private DelegateVertexAndEdgeListGraph<IAssembly, SEquatableEdge<IAssembly>> CreateGraph()
        {
            var graphDictionary = new AsmReference(asmReference.Keys);
            foreach (var names in asmReference.Values)
            {
                foreach (var name in names.Where(name => !graphDictionary.ContainsKey(name)))
                {
                    graphDictionary.Add(name);
                }
            }
            foreach (var assembly in asmReference)
            {
                graphDictionary[assembly.Key] = assembly.Value;
            }

            var graph =
                graphDictionary.ToVertexAndEdgeListGraph(kv => kv.Value.Select(v => new SEquatableEdge<IAssembly>(kv.Key, v)));
            return graph;
        }        
    }
}