using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Mono.Cecil;

namespace Analyzer
{
    [DebuggerDisplay("{FullName}")]
    public class AssemblyAnalyzer
    {
        private readonly ModuleDefinition moduleDefinition;

        public AssemblyAnalyzer(string path)
        {
            moduleDefinition = ModuleDefinition.ReadModule(path);
        }

        public AssemblyAnalyzer(Assembly assembly) : this(assembly.Location)
        {
        }

        public IEnumerable<TypeAnalyzer> GetAllTypes()
        {
            var types = moduleDefinition.GetTypes();
            foreach (var typeDefinition in types)
            {
                yield return new TypeAnalyzer(typeDefinition);
            }
        }

        public string Name => moduleDefinition.Name;

        public string FullName => moduleDefinition.FullyQualifiedName;
    }
}
