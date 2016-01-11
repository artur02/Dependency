using System.Diagnostics;
using Analyzer.ReturnTypes;
using Mono.Cecil;

namespace Analyzer
{
    [DebuggerDisplay("{type}")]
    public class BaseTypeWalker
    {
        readonly TypeDefinition type;
        ComponentCache cache;

        public BaseTypeWalker(IType type, ComponentCache componentCache)
        {
            this.type = type.TypeDefinition;
            cache = componentCache;
        }

        public TypeReferenceCount GetBaseTypes()
        {
            var types = new TypeReferenceCount
            {
                GetBaseClasses(),
                GetInterfaces()
            };

            return types;
        }

        public TypeReferenceCount GetBaseClasses()
        {
            var types = new TypeReferenceCount();
            if (type.BaseType != null)
            {
                types.Add(new Type(type.BaseType, cache));
            }

            return types;
        }

        public TypeReferenceCount GetInterfaces()
        {
            var types = new TypeReferenceCount();
            foreach (var interf in type.Interfaces)
            {
                types.Add(new Type(interf, cache));
            }
            return types;
        }
    }
}