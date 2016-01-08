using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;

namespace Analyzer
{
    [DebuggerDisplay("{FullName}")]
    public class TypeAnalyzer
    {
        private readonly TypeDefinition typeDefinition;

        public TypeAnalyzer(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
        }

        public TypeAnalyzer(TypeReference typeReference) : this(typeReference.Resolve())
        {
        }

        public string Name => typeDefinition.Name;

        public string FullName => typeDefinition.FullName;

        public IEnumerable<TypeAnalyzer> GetDependencies()
        {
            foreach (var methodAnalyzer in typeDefinition.Methods.Select(method => new MethodAnalyzer(method)))
            {
                foreach (var dependency in methodAnalyzer.GetDependencies())
                {
                    yield return dependency;
                }
            }
        }
    }
}
