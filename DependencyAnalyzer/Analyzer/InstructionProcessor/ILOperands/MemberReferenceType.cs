using Mono.Cecil;

namespace Analyzer.InstructionProcessor.ILOperands
{
    public class MemberReferenceType : IOperandType
    {
        private readonly object operand;

        public MemberReferenceType(object operand)
        {
            this.operand = operand;
        }

        public Type GetOperandType()
        {
            var reference = operand as MemberReference;
            if (reference != null)
            {
                var op = reference;
                var declaringType = op.DeclaringType;
                if (declaringType != null)
                {
                    return new Type(declaringType, new ComponentCache());
                }
            }

            return null;
        }
    }
}
