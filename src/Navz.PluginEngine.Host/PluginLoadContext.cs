using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;

namespace Navz.PluginEngine.Host
{
    /// <summary>
    /// Provides assembly load context isolation for plugins, enabling dynamic loading and unloading of plugin assemblies with caching support.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public sealed class PluginLoadContext : AssemblyLoadContext, IDisposable
    {
        private readonly string _pluginPath;
        private readonly ConcurrentDictionary<string, WeakReference<Assembly>> _loadedAssemblies;
        private bool _disposed;

        /// <summary>
        /// Gets the number of assemblies currently cached in this load context.
        /// </summary>
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access", Justification = "Assembly cache is preserved during trimming")]
        public int CachedAssemblyCount => _loadedAssemblies.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadContext"/> class for the specified plugin assembly path.
        /// </summary>
        /// <param name="pluginPath">The absolute path to the plugin assembly (.dll).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="pluginPath"/> is null.</exception>
        public PluginLoadContext(string pluginPath) : base(isCollectible: true)
        {
            ArgumentNullException.ThrowIfNull(pluginPath);
            _pluginPath = pluginPath;
            _loadedAssemblies = new ConcurrentDictionary<string, WeakReference<Assembly>>();
        }

        /// <summary>
        /// Loads the requested assembly into the plugin's load context.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to load.</param>
        /// <returns>The loaded assembly, or null if not found.</returns>
        /// <remarks>
        /// Shared abstractions are always loaded from the default context to ensure type compatibility.
        /// </remarks>
        [RequiresUnreferencedCode("Assembly loading may require types that cannot be statically analyzed")]
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access", Justification = "Plugin loading is inherently dynamic")]
        [RequiresDynamicCode("Loading assemblies requires dynamic code that cannot be statically analyzed")]
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            ArgumentNullException.ThrowIfNull(assemblyName);

            // Always use the default context for shared abstractions to ensure type compatibility
            if (assemblyName.Name == "Navz.PluginEngine.Abstractions")
            {
                return null; // This will fall back to the default context
            }

            // Check if the assembly is already loaded and cached
            if (_loadedAssemblies.TryGetValue(assemblyName.FullName, out var weakRef))
            {
                if (weakRef.TryGetTarget(out var cachedAssembly))
                {
                    return cachedAssembly;
                }
                // Remove the weak reference if the assembly was collected
                _loadedAssemblies.TryRemove(assemblyName.FullName, out _);
            }

            string assemblyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_pluginPath)!, $"{assemblyName.Name}.dll");
            if (System.IO.File.Exists(assemblyPath))
            {
                var assembly = LoadFromAssemblyPath(assemblyPath);
                // Cache the loaded assembly using a weak reference
                _loadedAssemblies.TryAdd(assemblyName.FullName, new WeakReference<Assembly>(assembly));
                return assembly;
            }
            return null;
        }

        /// <summary>
        /// Clears the assembly cache for this load context. Does not unload assemblies.
        /// </summary>
        public void ClearCache()
        {
            _loadedAssemblies.Clear();
        }

        /// <summary>
        /// Disposes the plugin load context, clearing the cache and unloading assemblies.
        /// </summary>
        /// <remarks>
        /// After disposal, the context cannot be used for further assembly loading.
        /// </remarks>
        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    ClearCache();
                    Unload();
                }
                catch (InvalidOperationException)
                {
                    // Ignore errors during unload as the context might already be unloaded
                }
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
