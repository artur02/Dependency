using System;
using System.Diagnostics.Contracts;
using Mono.Cecil;

namespace Analyzer
{
    public class Parameter : Type
    {
        readonly ParameterDefinition parameter;
        readonly ComponentCache cache;

        public Parameter(ParameterDefinition parameterDefinition, ComponentCache componentCache) : base(parameterDefinition.ParameterType, componentCache)
        {
            Contract.Requires<ArgumentNullException>(parameterDefinition != null);

            parameter = parameterDefinition;
            cache = componentCache;
        }

        public Parameter(ParameterReference parameterReference, ComponentCache componentCache) : base(parameterReference.ParameterType, componentCache)
        {
            Contract.Requires<ArgumentNullException>(parameterReference != null);

            parameter = parameterReference.Resolve();
            cache = componentCache;
        }
    }
}