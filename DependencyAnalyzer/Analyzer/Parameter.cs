using Mono.Cecil;

namespace Analyzer
{
    public class Parameter : Type
    {
        ParameterDefinition parameter;
        ComponentCache cache;

        public Parameter(ParameterDefinition parameterDefinition, ComponentCache componentCache) : base(parameterDefinition.ParameterType, componentCache)
        {
            parameter = parameterDefinition;
            cache = componentCache;
        }

        public Parameter(ParameterReference parameterReference, ComponentCache componentCache) : base(parameterReference.ParameterType, componentCache)
        {
            parameter = parameterReference.Resolve();
            cache = componentCache;
        }
    }
}