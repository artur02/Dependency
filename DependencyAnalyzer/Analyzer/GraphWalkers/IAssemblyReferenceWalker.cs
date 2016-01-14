using Analyzer.ReturnTypes;

namespace Analyzer.GraphWalkers
{
    public interface IAssemblyReferenceWalker
    {
        uint RecursionLimit { get; set; }
        AsmReference References { get; }
    }
}