using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using log4net;
using Mono.Cecil;

namespace Analyzer
{
    [DebuggerDisplay("{FullyQualifiedName}")]
    public class Assembly : IEquatable<IAssembly>, IAssembly
    {
        readonly ModuleDefinition module;
        readonly DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();

        readonly AssemblyReferenceWalker referenceWalker;
        ComponentCache cache;

        private readonly ILog logger = LogManager.GetLogger(typeof (Assembly));

        private Assembly(ComponentCache componentCache)
        {
            cache = componentCache;
            referenceWalker = new AssemblyReferenceWalker(this);
        }

        /// <summary>
        /// Creates a new instance of the Assembly class
        /// </summary>
        /// <param name="path">File path to the assembly</param>
        public Assembly(string path, ComponentCache componentCache)
            : this(componentCache)
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
        public Assembly(AssemblyNameReference assemblyName, ComponentCache componentCache)
            : this(componentCache)
        {
            Contract.Requires<ArgumentNullException>(assemblyName != null);
            Contract.Ensures(module != null);

            var asm = resolver.Resolve(assemblyName);
            module = asm.MainModule;
        }

        public Assembly(ModuleDefinition assembly, ComponentCache componentCache)
            : this(componentCache)
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
        public Assembly(AssemblyNameReference assemblyName, IEnumerable<string> searchDirectories, ComponentCache componentCache)
        {
            cache = componentCache;
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

        public ModuleDefinition Module => module;

        /// <summary>
        /// Returns the assembly names referenced directly by the current assembly
        /// </summary>
        public IEnumerable<IAssembly> References
        {
            get
            {
                var assemblies = new List<IAssembly>();

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

                return assemblies;
            }
        }

        public string FullyQualifiedName => module.FullyQualifiedName;

        /// <summary>
        /// Returns the referenced assemblies recursively
        /// </summary>
        /// <param name="recursionLimit">Recursion limit of the operation</param>
        /// <returns></returns>
        public ReturnTypes.AsmReference GetReferences(uint recursionLimit = uint.MaxValue)
        {
            referenceWalker.RecursionLimit = recursionLimit;
            return referenceWalker.References;
        }

        public Hashtable GetReferencesAsHashtable(uint recursionLimit = uint.MaxValue)
        {

            var referencesDict = GetReferences(recursionLimit);
            var referencesHash = new Hashtable(referencesDict);

            return referencesHash;
        }

        public IEnumerable<IType> GetTypes()
        {
            var types = module.GetTypes();
            foreach (var typeDefinition in types)
            {
                yield return new Type(typeDefinition, cache);
            }
        }

        public string Name
        {
            get
            {
                var moduleName = Path.GetFileNameWithoutExtension(module.Name);
                return moduleName;
            }
        }

        public bool Equals(IAssembly other)
        {
            return module.FullyQualifiedName == other.Module.FullyQualifiedName;
        }

        public override string ToString()
        {
            return $"Name: {Module.FullyQualifiedName}";
        }

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