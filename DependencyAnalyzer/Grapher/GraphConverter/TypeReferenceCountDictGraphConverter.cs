using System.Linq;
using Analyzer.ReturnTypes;
using Grapher.GraphMarkup;
using QuickGraph;

namespace Grapher.GraphConverter
{
    public class TypeReferenceCountDictGraphConverter : IGraphConverter
    {
        readonly TypeReferenceCountDict typeReferenceCountDict;
        private readonly IGraphMarkup<IType> markup;

        public TypeReferenceCountDictGraphConverter(TypeReferenceCountDict typeReferenceCountDict, IGraphMarkup<IType> markup = null)
        {
            this.typeReferenceCountDict = typeReferenceCountDict;
            this.markup = markup;
        }

        public string Convert()
        {
            var graph = CreateGraph();
            var result = markup.Serialize(graph, v => v.FullName);

            return result;
        }

        private DelegateVertexAndEdgeListGraph<IType, SEquatableEdge<IType>> CreateGraph()
        {
            var graphDictionary = new TypeReferenceCountDict(typeReferenceCountDict.Keys);
            foreach (var names in typeReferenceCountDict.Values.Select(v => v.Keys))
            {
                foreach (var name in names.Where(name => !graphDictionary.ContainsKey(name)))
                {
                    graphDictionary.Add(name);
                }
            }
            foreach (var assembly in typeReferenceCountDict)
            {
                graphDictionary[assembly.Key] = assembly.Value;
            }

            var graph =
                graphDictionary.ToVertexAndEdgeListGraph(
                    kv => kv.Value.Select(v => new SEquatableEdge<IType>(kv.Key, v.Key)));
            return graph;
        }
    }
}