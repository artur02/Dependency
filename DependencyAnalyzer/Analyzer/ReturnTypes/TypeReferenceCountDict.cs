using System;
using System.Collections.Generic;

namespace Analyzer.ReturnTypes
{
    [Serializable]
    public class TypeReferenceCountDict : Dictionary<IType, TypeReferenceCount>
    {
        public TypeReferenceCountDict()
        { }

        public TypeReferenceCountDict(IEnumerable<IType> types)
        {
            foreach (var type in types)
            {
                Add(type);
            }
        }

        public void Add(IType referer)
        {
            if (!ContainsKey(referer))
            {
                Add(referer, new TypeReferenceCount());
            }
        }
    }
}