using System;
using Mono.Cecil;

namespace Analyzer.InstructionProcessor.ILOperands
{
    public class PrimitiveType : IOperandType
    {
        private readonly object operand;
        readonly ComponentCache cache = new ComponentCache();

        public PrimitiveType(object operand)
        {
            this.operand = operand;
        }

        /// <exception cref="SecurityException">The caller does not have the required permissions. </exception>
        public Type GetOperandType()
        {
            if (operand is sbyte || operand is byte || operand is Int32 || operand is Int64 || operand is Single)
            {
                var corlib = cache.ModuleCache.Resolve(typeof(object).Module.FullyQualifiedName);

                var t = operand.GetType();
                var tr = new TypeReference(t.Namespace, t.Name, corlib, corlib, true);
                return new Type(tr, cache);
            }

            return null;
        }
    }
}
