using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Analyzer.ReturnTypes;
using log4net;
using Mono.Cecil;
using Mono.Cecil.Cil;

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
            method = methodReference.Resolve();
            cache = componentCache;
        }

        public Method(MethodDefinition methodDefinition, ComponentCache componentCache)
        {
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

            if (method.Body != null)
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
                        types.Add(new Type(variableDefinition.VariableType, cache));
                    }
                }

                foreach (var instruction in method.Body.Instructions)
                {
                    var operand = instruction.Operand;
                    if (operand != null)
                    {
                        if (operand is MemberReference)
                        {
                            var op = operand as MemberReference;
                            var dt = op.DeclaringType;
                            if (dt != null)
                            {
                                types.Add(new Type(dt, cache));
                            }
                        }
                        else if (operand is ParameterDefinition)
                        {
                            var par = operand as ParameterDefinition;
                            var partype = par.ParameterType;
                            types.Add(new Type(partype, cache));
                        }
                        else if (operand is VariableDefinition)
                        {
                            var vari = operand as VariableDefinition;
                            var varitype = vari.VariableType;

                            if (varitype.IsGenericParameter)
                            {
                                logger.Warn($"Generic parameter not processed: {varitype.FullName}");
                            }
                            else
                            {
                                types.Add(new Type(varitype, cache));
                            }
                        }
                        else if (operand is sbyte || operand is byte || operand is Int32 || operand is Int64 || operand is Single)
                        {
                            //var corlib = ModuleDefinition.ReadModule(typeof(object).Module.FullyQualifiedName);
                            var corlib = cache.ModuleCache.Resolve(typeof(object).Module.FullyQualifiedName);

                            var t = operand.GetType();
                            var tr = new TypeReference(t.Namespace, t.Name, corlib, corlib, true);
                            types.Add(new Type(tr, cache));
                        }
                    }
                }
            }

            return types;
        }
    }
}