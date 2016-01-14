using System.IO;
using System.Text;
using System.Xml;
using Analyzer;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Serialization;

namespace Grapher.GraphMarkup
{
    /// <summary>
    /// GraphML markup
    /// </summary>
    public class GraphML : IGraphMarkup
    {
        /// <summary>
        /// Serializes graph into MathML markup
        /// </summary>
        /// <param name="graph">Graph representation</param>
        /// <returns>GraphML markup</returns>
        public string Serialize(DelegateVertexAndEdgeListGraph<IAssembly, SEquatableEdge<IAssembly>> graph)
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
