using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;
using Analyzer.ReturnTypes;
using Mono.Cecil;

namespace Analyzer
{
    /// <summary>
    /// Represents a .NET assembly
    /// </summary>
    [ContractClass(typeof(IAssemblyContract))]
    public interface IAssembly
    {
        /// <summary>
        /// A module definition describing the assembly
        /// </summary>
        ModuleDefinition Module { get; }

        /// <summary>
        /// Returns the assembly names referenced directly by the current assembly
        /// </summary>
        IEnumerable<IAssembly> References { get; }

        /// <summary>
        /// The nsmae of the assembly
        /// </summary>
        [XmlAttribute("name")]
        string Name { get; }

        /// <summary>
        /// The fully qualified name of the assembly
        /// </summary>
        [XmlAttribute("fullyQualifiedName")]
        string FullyQualifiedName { get; }

        /// <summary>
        /// Returns the referenced assemblies recursively
        /// </summary>
        /// <param name="recursionLimit">The number of the followed references</param>
        /// <returns></returns>
        AsmReference GetReferences(uint recursionLimit);

        /// <summary>
        /// Returns the referenced assemblies recursively as a Hashtable
        /// </summary>
        /// <param name="recursionLimit">The number of the followed references</param>
        /// <returns></returns>
        Hashtable GetReferencesAsHashtable(uint recursionLimit);

        /// <summary>
        /// Returns the types defined in the assembly
        /// </summary>
        /// <returns>The enumeration of the published types</returns>
        IEnumerable<IType> GetTypes();

        /// <summary>
        /// Determines if two assemblies are the same
        /// </summary>
        /// <param name="other">The other assembly</param>
        /// <returns></returns>
        bool Equals(IAssembly other);

        /// <summary>
        /// The description of the assembly
        /// </summary>
        /// <returns>The description.</returns>
        string ToString();
    }

    [ContractClassFor(typeof(IAssembly))]
    abstract class IAssemblyContract : IAssembly
    {
        public ModuleDefinition Module { get; private set; }
        public IEnumerable<IAssembly> References { get; private set; }
        public string Name { get; private set; }
        public string FullyQualifiedName { get; private set; }
        public AsmReference GetReferences(uint recursionLimit)
        {
            Contract.Ensures(Contract.Result<AsmReference>() != null);

            return default(AsmReference);
        }

        public Hashtable GetReferencesAsHashtable(uint recursionLimit)
        {
            Contract.Ensures(Contract.Result<Hashtable>() != null);

            return default(Hashtable);
        }

        public IEnumerable<IType> GetTypes()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IType>>() != null);

            return default(IEnumerable<IType>);
        }

        public bool Equals(IAssembly other)
        {
            return default(bool);
        }
    }
}