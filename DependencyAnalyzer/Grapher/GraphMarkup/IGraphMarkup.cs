using System;
using System.Diagnostics.Contracts;
using QuickGraph;

namespace Grapher.GraphMarkup
{
    /// <summary>
    /// Graph markup interface
    /// </summary>
    [ContractClass(typeof(IGraphMarkupContract<>))]
    public interface IGraphMarkup<T>
    {
        /// <summary>
        /// Serializes graph into a graph representation markup
        /// </summary>
        /// <param name="graph">Graph representation</param>
        /// <param name="vertexIdentity">The function that returns the identity property of the current vertex (e.g. Name)</param>
        /// <returns>Graph representation markup</returns>
        string Serialize(DelegateVertexAndEdgeListGraph<T, SEquatableEdge<T>> graph, VertexIdentity<T> vertexIdentity);
    }

    /// <summary>
    /// Graph markup code contracts
    /// </summary>
    [ContractClassFor(typeof(IGraphMarkup<>))]
    public abstract class IGraphMarkupContract<T> : IGraphMarkup<T>
    {
        /// <summary>
        /// Serializes graph into a graph representation markup
        /// </summary>
        /// <param name="graph">Graph representation</param>
        /// <param name="vertexIdentity">The function that returns the identity property of the current vertex (e.g. Name)</param>
        /// <returns>Graph representation markup</returns>
        public string Serialize(DelegateVertexAndEdgeListGraph<T, SEquatableEdge<T>> graph, VertexIdentity<T> vertexIdentity)
        {
            Contract.Requires<ArgumentNullException>(graph != null);

            return default(string);
        }
    }
}