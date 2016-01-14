using log4net;
using Mono.Cecil.Cil;

namespace Analyzer.InstructionProcessor.ILOperands
{
    public class VariableDefinitionType : IOperandType
    {
        private readonly object operand;
        private readonly ILog logger = LogManager.GetLogger(typeof (VariableDefinitionType));

        public VariableDefinitionType(object operand)
        {
            this.operand = operand;
        }

        public Type GetOperandType()
        {
            var variableDefinition = operand as VariableDefinition;
            if (variableDefinition != null)
            {
                var variableType = variableDefinition.VariableType;
                if (variableType.IsGenericParameter)
                {
                    logger.Warn($"Generic parameter not processed: {variableType.FullName}");
                }
                else
                {
                    return new Type(variableType, new ComponentCache());
                }
            }

            return null;
        }
    }
}
