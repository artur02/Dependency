using Mono.Cecil;

namespace Analyzer.InstructionProcessor.ILOperands
{
    public class ParameterDefinitionType : IOperandType
    {
        private readonly object operand;

        public ParameterDefinitionType(object operand)
        {
            this.operand = operand;
        }

        public Type GetOperandType()
        {
            var par = operand as ParameterDefinition;
            if (par != null)
            {
                var partype = par.ParameterType;
                return new Type(partype, new ComponentCache());
            }

            return null;
        }
    }
}
