using System.IO;
using System.Text;
using System.Xml;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Serialization;

namespace Grapher.GraphMarkup
{
    /// <summary>
    /// GraphML markup
    /// </summary>
    public class GraphML<T> : IGraphMarkup<T>
    {
        /// <summary>
        /// Serializes graph into MathML markup
        /// </summary>
        /// <param name="graph">Graph representation</param>
        /// <param name="vertexIdentity">The function that returns the identity property of the current vertex (e.g. Name)</param>
        /// <returns>GraphML markup</returns>
        public string Serialize(DelegateVertexAndEdgeListGraph<T, SEquatableEdge<T>> graph, VertexIdentity<T> vertexIdentity)
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
                    graph.SerializeToGraphML(xmlWriter, vertexIdentity, graph.GetEdgeIdentity());
                }
                result = Encoding.UTF8.GetString(memoryStream.ToArray());

            }

            return result;
        }
    }
}
