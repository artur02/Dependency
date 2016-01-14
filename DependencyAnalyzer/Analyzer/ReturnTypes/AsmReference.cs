using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;

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
            Contract.Requires<ArgumentNullException>(assemblyReference != null);

            Add(assemblyReference);
        }

        /// <summary>
        /// Creates an instance of AsmReference
        /// </summary>
        /// <param name="assemblies"></param>
        public AsmReference(IEnumerable<IAssembly> assemblies)
            : this()
        {
            Contract.Requires<ArgumentNullException>(assemblies != null);

            foreach (var assembly in assemblies)
            {
                Add(assembly);
            }
        }

        /// <summary>
        /// Number of assemblies referring other assemblies
        /// </summary>
        [Pure]
        public int ReferrerCount => Count;

        /// <summary>
        /// Assemblies referring other assemblies
        /// </summary>
        [Pure]
        public IEnumerable<IAssembly> Referrers => Keys;

        public void Add(IAssembly referer)
        {
            Contract.Requires<ArgumentNullException>(referer != null);

            if (!ContainsKey(referer))
            {
                Add(referer, new HashSet<IAssembly>(new Assembly.AssemblyEqualityComparer()));
            }
        }

        public void Add(IAssembly referer, IAssembly referenced)
        {
            Contract.Requires<ArgumentNullException>(referer != null);
            Contract.Requires<ArgumentNullException>(referenced != null);

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
            Contract.Requires<ArgumentNullException>(referer != null);
            Contract.Requires<ArgumentNullException>(referenced != null);

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
            Contract.Requires<ArgumentNullException>(assemblyReference != null);

            var references = assemblyReference.ToList();
            foreach (var reference in references)
            {
                Add(reference.Key, reference.Value);
            }
        }

        [Pure]
        public bool ContainsReferer(IAssembly assembly)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);

            return ContainsKey(assembly);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(ReferrerCount >= 0);
            Contract.Invariant(Referrers != null);
        }
    }
}