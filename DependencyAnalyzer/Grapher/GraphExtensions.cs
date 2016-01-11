using Analyzer;
using QuickGraph;

namespace Grapher
{
    public static class GraphExtensions
    {
        public static VertexIdentity<T> GetAssemblyVertexIdentity<T>(this IVertexSet<T> graph) where T : IAssembly
        {
            return v => v.FullyQualifiedName;
        }

        public static VertexIdentity<T> GetTypeVertexIdentity<T>(this IVertexSet<T> graph) where T : IType
        {
            return v => v.FullName;
        }
    }
}
