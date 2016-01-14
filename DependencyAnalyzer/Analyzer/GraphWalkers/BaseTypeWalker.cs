using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Analyzer.ReturnTypes;
using Mono.Cecil;

namespace Analyzer.GraphWalkers
{
    [DebuggerDisplay("{type}")]
    public class BaseTypeWalker
    {
        readonly TypeDefinition type;
        readonly ComponentCache cache;

        public BaseTypeWalker(IType type, ComponentCache componentCache)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(componentCache != null);

            this.type = type.TypeDefinition;
            cache = componentCache;
        }

        [Pure]
        public TypeReferenceCount GetBaseTypes()
        {
            var types = new TypeReferenceCount
            {
                GetBaseClasses(),
                GetInterfaces()
            };

            return types;
        }

        [Pure]
        public TypeReferenceCount GetBaseClasses()
        {
            var types = new TypeReferenceCount();
            if (type.BaseType != null)
            {
                types.Add(new Type(type.BaseType, cache));
            }

            return types;
        }

        [Pure]
        public TypeReferenceCount GetInterfaces()
        {
            var types = new TypeReferenceCount();

            if (type.Interfaces != null)
            {
                foreach (var @interface in type.Interfaces)
                {
                    types.Add(new Type(@interface, cache));
                }
            }

            return types;
        }
    }
}