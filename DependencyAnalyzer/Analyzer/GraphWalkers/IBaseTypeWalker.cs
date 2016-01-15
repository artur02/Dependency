using Analyzer.ReturnTypes;

namespace Analyzer.GraphWalkers
{
    public interface IBaseTypeWalker
    {
        TypeReferenceCount GetBaseTypes();
        TypeReferenceCount GetBaseClasses();
        TypeReferenceCount GetInterfaces();
    }
}