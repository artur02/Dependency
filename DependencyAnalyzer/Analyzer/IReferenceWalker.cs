namespace Analyzer
{
    internal interface IReferenceWalker
    {
        ReturnTypes.AsmReference GetReferences(uint recursionLimit = uint.MaxValue);
    }
}