using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using Analyzer.GraphWalkers;
using log4net;
using Mono.Cecil;

namespace Analyzer
{
    [DebuggerDisplay("{FullyQualifiedName}")]
    public class Assembly : IEquatable<IAssembly>, IAssembly
    {
        readonly ModuleDefinition module;
        readonly DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();

        readonly IAssemblyReferenceWalker referenceWalker;
        readonly ComponentCache cache;

        private readonly ILog logger = LogManager.GetLogger(typeof (Assembly));

        private Assembly(ComponentCache componentCache, IAssemblyReferenceWalker assemblyReferenceWalker)
        {
            cache = componentCache ?? new ComponentCache();
            referenceWalker = assemblyReferenceWalker ?? new AssemblyReferenceWalker(this);
        }

        /// <summary>
        /// Creates a new instance of the Assembly class
        /// </summary>
        /// <param name="path">File path to the assembly</param>
        public Assembly(string path, ComponentCache componentCache = null, IAssemblyReferenceWalker assemblyReferenceWalker = null)
            : this(componentCache, assemblyReferenceWalker)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(path));
            Contract.Requires<ArgumentException>(File.Exists(path));

            module = ModuleDefinition.ReadModule(path);
            resolver.AddSearchDirectory(Path.GetDirectoryName(path));
        }

        /// <summary>
        /// Creates a new instance of the Assembly class
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        public Assembly(AssemblyNameReference assemblyName, ComponentCache componentCache = null, IAssemblyReferenceWalker assemblyReferenceWalker = null)
            : this(componentCache, assemblyReferenceWalker)
        {
            Contract.Requires<ArgumentNullException>(assemblyName != null);
            Contract.Ensures(module != null);

            var asm = resolver.Resolve(assemblyName);
            module = asm.MainModule;
        }

        public Assembly(ModuleDefinition assembly, ComponentCache componentCache = null, IAssemblyReferenceWalker assemblyReferenceWalker = null)
            : this(componentCache, assemblyReferenceWalker)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);
            Contract.Ensures(module != null);

            module = assembly;
        }

        /// <summary>
        /// Creates a new instance of the Assembly class
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="searchDirectories">Search directories</param>
        public Assembly(AssemblyNameReference assemblyName, IEnumerable<string> searchDirectories, ComponentCache componentCache = null, IAssemblyReferenceWalker assemblyReferenceWalker = null)
            : this(componentCache, assemblyReferenceWalker)
        {
            Contract.Requires<ArgumentNullException>(assemblyName != null);
            Contract.Requires<ArgumentNullException>(searchDirectories != null);
            Contract.Requires(Contract.ForAll(searchDirectories, d => d != null));
            Contract.Ensures(module != null);

            foreach (var searchDirectory in searchDirectories)
            {
                resolver.AddSearchDirectory(searchDirectory);
            }
            var asm = resolver.Resolve(assemblyName);
            module = asm.MainModule;
        }

        [Pure]
        public ModuleDefinition Module => module;

        /// <summary>
        /// Returns the assembly names referenced directly by the current assembly
        /// </summary>
        [Pure]
        public IEnumerable<IAssembly> References
        {
            get
            {
                var assemblies = new List<IAssembly>();

                if (module.AssemblyReferences != null)
                {
                    foreach (var reference in module.AssemblyReferences)
                    {
                        try
                        {
                            assemblies.Add(new Assembly(reference, resolver.GetSearchDirectories(), cache));
                        }
                        catch (Exception exception)
                        {
                            logger.Warn($"Failed processing assembly reference: {reference?.FullName}", exception);
                        }
                    }
                }

                return assemblies;
            }
        }

        /// <summary>
        /// The fully qualified name of the assembly
        /// </summary>
        [Pure]
        public string FullyQualifiedName => module.FullyQualifiedName;

        /// <summary>
        /// Returns the referenced assemblies recursively
        /// </summary>
        /// <param name="recursionLimit">Recursion limit of the operation</param>
        /// <returns></returns>
        [Pure]
        public ReturnTypes.AsmReference GetReferences(uint recursionLimit = uint.MaxValue)
        {
            referenceWalker.RecursionLimit = recursionLimit;
            return referenceWalker.References;
        }

        /// <summary>
        /// Returns the types defined in the assembly
        /// </summary>
        /// <returns>The enumeration of the published types</returns>
        [Pure]
        public IEnumerable<IType> GetTypes()
        {
            var types = module.GetTypes();
            foreach (var typeDefinition in types)
            {
                yield return new Type(typeDefinition, cache);
            }
        }

        /// <summary>
        /// The nsmae of the assembly
        /// </summary>
        [Pure]
        public string Name => Path.GetFileNameWithoutExtension(module.Name);

        /// <summary>
        /// Determines if two assemblies are the same
        /// </summary>
        /// <param name="other">The other assembly</param>
        /// <returns></returns>
        [Pure]
        public bool Equals(IAssembly other)
        {

            return module.FullyQualifiedName == other.Module.FullyQualifiedName;
        }

        /// <summary>
        /// The description of the assembly
        /// </summary>
        /// <returns>The description.</returns>
        [Pure]
        public override string ToString()
        {
            return $"Name: {Module.FullyQualifiedName}";
        }

        [Pure]
        public static bool IsAssembly(string path, out Exception exception)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(path));

            try
            {
                AssemblyName.GetAssemblyName(path);

                exception = null;
                return true;
            }
            catch (FileNotFoundException ex)    // File not found
            {
                exception = ex;
                return false;
            }
            catch (BadImageFormatException ex)  // Not an assembly
            {
                exception = ex;
                return false;
            }
            catch (FileLoadException)
            {
                exception = null;
                return true;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Name != null);
            Contract.Invariant(FullyQualifiedName != null);
        }

        [Pure]
        public class AssemblyEqualityComparer : IEqualityComparer<IAssembly>
        {
            public bool Equals(IAssembly x, IAssembly y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(IAssembly obj)
            {
                return obj.FullyQualifiedName.GetHashCode();
            }


        }
    }
}