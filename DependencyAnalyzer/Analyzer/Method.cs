using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Analyzer.InstructionProcessor;
using Analyzer.InstructionProcessor.ILOperands;
using Analyzer.ReturnTypes;
using log4net;
using Mono.Cecil;

namespace Analyzer
{
    [DebuggerDisplay("{FullName}")]
    public class Method
    {
        readonly MethodDefinition method;
        readonly ComponentCache cache;
        private readonly ILog logger = LogManager.GetLogger(typeof(Method));

        public Method(MethodReference methodReference, ComponentCache componentCache)
        {
            Contract.Requires<ArgumentNullException>(methodReference != null);
            Contract.Requires<ArgumentNullException>(componentCache != null);

            method = methodReference.Resolve();
            cache = componentCache;
        }

        public Method(MethodDefinition methodDefinition, ComponentCache componentCache)
        {
            Contract.Requires<ArgumentNullException>(methodDefinition != null);
            Contract.Requires<ArgumentNullException>(componentCache != null);

            method = methodDefinition;
            cache = componentCache;
        }

        public string Name => method.Name;
        public string FullName => method.FullName;


        [Pure]
        public TypeReferenceCount GetTypeReferences()
        {
            var types = new TypeReferenceCount();

            foreach (var parameter in GetParameters())
            {
                types.Add(parameter);
            }

            types.Add(GetBodyReferences());

            return types;
        }

        [Pure]
        public IEnumerable<Parameter> GetParameters()
        {
            foreach (var parameterDefinition in method.Parameters)
            {
                if (parameterDefinition.ParameterType.IsGenericParameter || parameterDefinition.ParameterType.ContainsGenericParameter)
                {
                    logger.Warn($"Cannot process generic parameters: {parameterDefinition.Name}");
                }
                else
                {
                    yield return new Parameter(parameterDefinition, cache);
                }
            }
        }

        [Pure]
        public TypeReferenceCount GetBodyReferences()
        {
            var types = new TypeReferenceCount();

            if (method.HasBody && method.Body != null)
            {
                foreach (var variable in GetMethodBodyVariables(method))
                {
                    types.Add(variable);
                }

                foreach (var instruction in method.Body.Instructions)
                {
                    var operand = instruction.Operand;
                    if (operand != null)
                    {
                        var operandProcessors = new IOperandType[]
                        {
                            new MemberReferenceType(operand),
                            new ParameterDefinitionType(operand),
                            new VariableDefinitionType(operand),
                            new PrimitiveType(operand), 
                        };

                        foreach (var operandProcessor in operandProcessors)
                        {
                            var type = operandProcessor.GetOperandType();
                            if (type != null)
                            {
                                types.Add(type);
                                break;
                            }
                        }
                    }
                }
            }

            return types;
        }

        [Pure]
        private IEnumerable<Type> GetMethodBodyVariables(MethodDefinition method)
        {
            Contract.Requires<ArgumentNullException>(method != null);

            if (method.HasBody && method.Body.HasVariables)
            {
                var variables = method.Body.Variables;
                foreach (var variableDefinition in variables)
                {
                    if (variableDefinition.VariableType.IsGenericParameter ||
                        variableDefinition.VariableType.ContainsGenericParameter)
                    {
                        logger.Warn($"Cannot process generic parameters: {variableDefinition.Name}");
                    }
                    else
                    {
                        yield return new Type(variableDefinition.VariableType, cache);
                    }
                }
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Name != null);
            Contract.Invariant(FullName!= null);
        }
    }
}