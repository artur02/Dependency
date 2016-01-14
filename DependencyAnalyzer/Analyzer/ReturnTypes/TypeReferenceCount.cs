using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Analyzer.ReturnTypes
{
    [Serializable]
    public class TypeReferenceCount : Dictionary<IType, UInt32>
    {
        public TypeReferenceCount() : base(new Type.TypeEqualityComparer())
        {
        }

        public TypeReferenceCount(IEnumerable<IType> types) : this()
        {
            Contract.Requires<ArgumentNullException>(types != null);

            foreach (var type in types)
            {
                Add(type);
            }
        }

        public void Add(IType key)
        {
            Contract.Requires<ArgumentNullException>(key != null);

            if (ContainsKey(key))
            {
                this[key] = this[key] + 1;
            }
            else
            {
                Add(key, 1);
            }
        }

        public void Add(TypeReferenceCount typeReferenceCount)
        {
            Contract.Requires<ArgumentNullException>(typeReferenceCount != null);

            foreach (var typeRef in typeReferenceCount)
            {
                if (ContainsKey(typeRef.Key))
                {
                    this[typeRef.Key] += typeRef.Value;
                }
                else
                {
                    Add(typeRef.Key, typeRef.Value);
                }
            }
        }
    }
}