using System;
using System.Diagnostics.Contracts;
using Analyzer.ReturnTypes;

namespace Analyzer.GraphWalkers
{
    public class AssemblyReferenceWalker : IAssemblyReferenceWalker
    {
        readonly IAssembly assembly;
        Lazy<AsmReference> references;
        uint recursionLimit;

        public AssemblyReferenceWalker(IAssembly assembly, uint recursionLimit = uint.MaxValue)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);

            this.assembly = assembly;
            this.recursionLimit = recursionLimit;
            references = new Lazy<AsmReference>(() =>
                GetReferences(assembly, new AsmReference(), recursionLimit)
                );
        }

        public uint RecursionLimit
        {
            get
            {
                return recursionLimit;
            }
            set
            {
                if (recursionLimit != value)
                {
                    recursionLimit = value;
                    references = new Lazy<AsmReference>(() =>
                        GetReferences(assembly, new AsmReference(), recursionLimit)
                        );
                }
            }
        }

        public AsmReference References
        {
            get
            {
                return references.Value;
            }
        }

        private AsmReference GetReferences(IAssembly currentAssembly, AsmReference referenceDictionary, uint currentRecursionLimit = uint.MaxValue)
        {
            Contract.Requires<ArgumentNullException>(currentAssembly != null);
            Contract.Requires<ArgumentNullException>(referenceDictionary != null);

            if (referenceDictionary.ContainsKey(currentAssembly))
            {
                return referenceDictionary;
            }

            if (currentRecursionLimit == 0)
            {
                return referenceDictionary;
            }

            referenceDictionary.Add(currentAssembly, currentAssembly.References);
            currentRecursionLimit--;
            foreach (var referencedAssembly in currentAssembly.References)
            {
                var assemblyReferences = GetReferences(referencedAssembly, referenceDictionary, currentRecursionLimit);
                referenceDictionary.Add(assemblyReferences);
            }

            return referenceDictionary;
        }
    }
}