using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Analyzer.ReturnTypes
{
    [Serializable]
    public class TypeReferenceCountDict : Dictionary<IType, TypeReferenceCount>
    {
        public TypeReferenceCountDict()
        { }

        public TypeReferenceCountDict(IEnumerable<IType> types)
        {
            Contract.Requires<ArgumentNullException>(types != null);

            foreach (var type in types)
            {
                Add(type);
            }
        }

        public void Add(IType referer)
        {
            Contract.Requires<ArgumentNullException>(referer != null);

            if (!ContainsKey(referer))
            {
                Add(referer, new TypeReferenceCount());
            }
        }
    }
}