using System.Runtime.Caching;
using Mono.Cecil;

namespace Analyzer
{
    public class ComponentCache
    {
        public ComponentCache()
        {
            ModuleCache = new ModuleCache();
        }

        public ModuleCache ModuleCache { get; set; }
    }

    public class ModuleCache
    {
        readonly ObjectCache cache = MemoryCache.Default;
        readonly CacheItemPolicy policy = new CacheItemPolicy();
        const string Prefix = "Module";

        public ModuleDefinition Resolve(string fileName)
        {
            var key = $"{Prefix}_{fileName}";
            if (!cache.Contains(key))
            {
                var module = ModuleDefinition.ReadModule(fileName);
                cache.Add(new CacheItem(key, module), policy);
            }

            return cache[key] as ModuleDefinition;
        }
    }
}