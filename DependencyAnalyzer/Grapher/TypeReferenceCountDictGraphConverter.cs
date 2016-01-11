using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Analyzer.ReturnTypes;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Serialization;

namespace Grapher
{
    public class TypeReferenceCountDictGraphConverter : IGraphConverter
    {
        readonly TypeReferenceCountDict typeReferenceCountDict;

        public TypeReferenceCountDictGraphConverter(TypeReferenceCountDict typeReferenceCountDict)
        {
            this.typeReferenceCountDict = typeReferenceCountDict;
        }

        public string Convert()
        {
            var graphDictionary = new TypeReferenceCountDict(typeReferenceCountDict.Keys);
            foreach (var names in typeReferenceCountDict.Values.Select(v => v.Keys))
            {
                foreach (var name in names.Where(name => !graphDictionary.ContainsKey(name)))
                {
                    graphDictionary.Add(name);
                }
            }
            foreach (var assemby in typeReferenceCountDict)
            {
                graphDictionary[assemby.Key] = assemby.Value;
            }

            var graph2 = graphDictionary.ToVertexAndEdgeListGraph(kv => kv.Value.Select(v => new SEquatableEdge<IType>(kv.Key, v.Key)));

            var result = SerializeToGraphML(graph2);

            return result;
        }

        string SerializeToGraphML(DelegateVertexAndEdgeListGraph<IType, SEquatableEdge<IType>> graph)
        {
            string result;

            using (var sw = new MemoryStream())
            {
                var xmlSettings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = true
                };
                using (var xw = XmlWriter.Create(sw, xmlSettings))
                {
                    graph.SerializeToGraphML(xw, graph.GetTypeVertexIdentity(), graph.GetEdgeIdentity());
                }
                result = Encoding.UTF8.GetString(sw.ToArray());
            }

            return result;
        }
    }
}