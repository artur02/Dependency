﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Analyzer.GraphWalkers;
using Analyzer.ReturnTypes;
using Mono.Cecil;

namespace Analyzer
{
    [DebuggerDisplay("{FullName}")]
    public class Type : IEquatable<IType>, IType
    {
        readonly TypeDefinition type;
        ComponentCache cache;

        public Type(TypeDefinition type, ComponentCache componentCache)
        {
            Contract.Requires(type != null);

            this.type = type;
            cache = componentCache;
        }

        public Type(TypeReference type, ComponentCache componentCache)
        {
            Contract.Requires(type != null);
            Contract.Ensures(this.type != null);

            this.type = type.Resolve();
            cache = componentCache;
        }

        public TypeDefinition TypeDefinition => type;

        [Pure]
        public TypeReferenceCount GetReferencedTypes()
        {
            var types = new TypeReferenceCount
            {
                GetTypesReferencedByMethods(),
                GetBaseTypes()
            };


            return types;
        }

        [Pure]
        public TypeReferenceCount GetBaseTypes()
        {
            var walker = new BaseTypeWalker(this, cache);
            return walker.GetBaseTypes();
        }

        [Pure]
        public IEnumerable<Method> GetMethods()
        {
            return type.Methods.Select(m => new Method(m, cache));
        }

        [Pure]
        public TypeReferenceCount GetTypesReferencedByMethods()
        {
            var types = new TypeReferenceCount();

            foreach (var method in GetMethods())
            {
                foreach (var typeReference in method.GetTypeReferences())
                {
                    types.Add(typeReference.Key, typeReference.Value);
                }
            }

            return types;
        }

        [Pure]
        public IEnumerable<IAssembly> GetUsedAssemblies()
        {
            var assemblies = new HashSet<IAssembly>(new Assembly.AssemblyEqualityComparer());

            var referencedTypes = GetReferencedTypes();
            foreach (var referencedType in referencedTypes)
            {
                assemblies.Add(referencedType.Key.Assembly);
            }

            return assemblies;
        }

        [Pure]
        public IAssembly Assembly => new Assembly(type.Module, cache);

        [Pure]
        public string FullName => type.FullName;

        [Pure]
        public bool Equals(IType other)
        {
            return FullName == other.FullName;
        }

        [Pure]
        public override bool Equals(object obj)
        {
            if (obj is IType)
            {
                var other = (IType) obj;
                return FullName == other.FullName;
            }

            return base.Equals(obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return type.FullName.GetHashCode();
        }

        [Pure]
        public override string ToString()
        {
            return $"Name: {FullName}";
        }

        [Pure]
        public class TypeEqualityComparer : IEqualityComparer<IType>
        {
            public bool Equals(IType x, IType y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(IType obj)
            {
                return obj.FullName.GetHashCode();
            }
        }
    }
}