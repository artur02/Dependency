using System.Collections.Generic;
using System.Diagnostics;
using Mono.Cecil;

namespace Analyzer
{
    [DebuggerDisplay("{Name}")]
    public class ParameterAnalyzer
    {
        private readonly ParameterDefinition parameterDefinition;

        public ParameterAnalyzer(ParameterDefinition parameterDefinition)
        {
            this.parameterDefinition = parameterDefinition;
        }

        public IEnumerable<TypeAnalyzer> GetDependencies()
        {
            yield return new TypeAnalyzer(parameterDefinition.ParameterType);
        }

        public string Name => parameterDefinition.Name;
    }
}
