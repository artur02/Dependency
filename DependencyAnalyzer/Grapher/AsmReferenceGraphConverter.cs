using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Analyzer;
using Analyzer.ReturnTypes;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Serialization;

namespace Grapher
{
    public class AsmReferenceGraphConverter : IGraphConverter
    {
        readonly AsmReference asmReference;

        public AsmReferenceGraphConverter(AsmReference asmReference)
        {
            this.asmReference = asmReference;
        }

        public string Convert()
        {
            var graphDictionary = new AsmReference(asmReference.Keys);
            foreach (var names in asmReference.Values)
            {
                foreach (var name in names.Where(name => !graphDictionary.ContainsKey(name)))
                {
                    graphDictionary.Add(name);
                }
            }
            foreach (var assemby in asmReference)
            {
                graphDictionary[assemby.Key] = assemby.Value;
            }

            var graph = graphDictionary.ToVertexAndEdgeListGraph(kv => kv.Value.Select(v => new SEquatableEdge<IAssembly>(kv.Key, v)));

            var result = SerializeToGraphML(graph);

            return result;
        }

        string SerializeToGraphML(DelegateVertexAndEdgeListGraph<IAssembly, SEquatableEdge<IAssembly>> graph)
        {
            string result;

            using (var memoryStream = new MemoryStream())
            {

                var xmlSettings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = true
                };
                using (var xmlWriter = XmlWriter.Create(memoryStream, xmlSettings))
                {
                    graph.SerializeToGraphML(xmlWriter, graph.GetAssemblyVertexIdentity(), graph.GetEdgeIdentity());
                }
                result = Encoding.UTF8.GetString(memoryStream.ToArray());

            }

            return result;
        }
    }
}