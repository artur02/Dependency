using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Analyzer
{
    public class MethodAnalyzer
    {
        private readonly MethodDefinition methodDefinition;

        public MethodAnalyzer(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
        }

        public string Name => methodDefinition.Name;

        public string FullName => methodDefinition.FullName;

        public IEnumerable<TypeAnalyzer> GetDependencies()
        {
            //if (methodDefinition.HasBody)
            //{
            //    foreach (var instruction in methodDefinition.Body.Instructions)
            //    {
            //        if (instruction.OpCode.OperandType == OperandType.InlineType)
            //        {
            //            yield return instruction.;
            //        }
            //    }
            //}

            if (methodDefinition.HasParameters)
            {
                foreach (var parameterAnalyzer in methodDefinition.Parameters.Select(parameter => new ParameterAnalyzer(parameter)))
                {
                    foreach (var dependency in parameterAnalyzer.GetDependencies())
                    {
                        yield return dependency;
                    }
                    
                } 
                
            }
        }
    }
}
