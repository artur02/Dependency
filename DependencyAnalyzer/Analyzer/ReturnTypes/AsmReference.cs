using System;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer.ReturnTypes
{
    /// <summary>
    /// An assembly reference dictionary that lists assemblies with the list of referenced assemblies
    /// </summary>
    [Serializable]
    public class AsmReference : Dictionary<IAssembly, HashSet<IAssembly>>
    {
        /// <summary>
        /// Creates an instance of AsmReference
        /// </summary>
        public AsmReference() : base(new Assembly.AssemblyEqualityComparer())
        { }

        /// <summary>
        /// Creates an instance of AsmReference
        /// </summary>
        /// <param name="assemblyReference">Another assembly reference dictionary that will be merged with the current one</param>
        public AsmReference(AsmReference assemblyReference)
            : this()
        {
            Add(assemblyReference);
        }

        /// <summary>
        /// Creates an instance of AsmReference
        /// </summary>
        /// <param name="assemblies"></param>
        public AsmReference(IEnumerable<IAssembly> assemblies)
            : this()
        {
            foreach (var assembly in assemblies)
            {
                Add(assembly);
            }
        }

        public void Add(IAssembly referer)
        {
            if (!ContainsKey(referer))
            {
                Add(referer, new HashSet<IAssembly>(new Assembly.AssemblyEqualityComparer()));
            }
        }

        public void Add(IAssembly referer, IAssembly referenced)
        {
            if (!ContainsKey(referer))
            {
                Add(referer, new HashSet<IAssembly>(new Assembly.AssemblyEqualityComparer())
                {
                    referenced
                });
            }
            else
            {
                this[referer].Add(referenced);
            }
        }

        public void Add(IAssembly referer, IEnumerable<IAssembly> referenced)
        {
            if (!ContainsKey(referer))
            {
                base.Add(referer, new HashSet<IAssembly>(referenced, new Assembly.AssemblyEqualityComparer()));
            }
            else
            {
                foreach (var assembly in referenced)
                {
                    this[referer].Add(assembly);
                }
            }
        }

        public void Add(AsmReference assemblyReference)
        {
            var references = assemblyReference.ToList();
            foreach (var reference in references)
            {
                Add(reference.Key, reference.Value);
            }
        }

        public IEnumerable<IAssembly> Referers => Keys;

        public bool ContainsReferer(IAssembly assembly)
        {
            return ContainsKey(assembly);
        }

        public int RefererCount => Count;
    }
}