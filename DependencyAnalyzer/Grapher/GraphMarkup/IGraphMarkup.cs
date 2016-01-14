using System;
using System.Diagnostics.Contracts;
using Analyzer;
using QuickGraph;

namespace Grapher.GraphMarkup
{
    /// <summary>
    /// Graph markup interface
    /// </summary>
    [ContractClass(typeof(IGraphMarkupContract))]
    public interface IGraphMarkup
    {
        /// <summary>
        /// Serializes graph into a graph representation markup
        /// </summary>
        /// <param name="graph">Graph representation</param>
        /// <returns>Graph representation markup</returns>
        string Serialize(DelegateVertexAndEdgeListGraph<IAssembly, SEquatableEdge<IAssembly>> graph);
    }

    /// <summary>
    /// Graph markup code contracts
    /// </summary>
    [ContractClassFor(typeof(IGraphMarkup))]
    public abstract class IGraphMarkupContract : IGraphMarkup
    {
        /// <summary>
        /// Serializes graph into a graph representation markup
        /// </summary>
        /// <param name="graph">Graph representation</param>
        /// <returns>Graph representation markup</returns>
        public string Serialize(DelegateVertexAndEdgeListGraph<IAssembly, SEquatableEdge<IAssembly>> graph)
        {
            Contract.Requires<ArgumentNullException>(graph != null);

            return default(string);
        }
    }
}