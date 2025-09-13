using Navz.PluginEngine.Abstractions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Navz.PluginEngine.Host
{
    /// <summary>
    /// Lightweight plugin manager optimized for minimal memory footprint and maximum performance.
    /// Provides essential plugin discovery, loading, and lifecycle management.
    /// </summary>
    public sealed class PluginManager : IDisposable
    {
        #region Fields
        // Optimized: Use arrays for better memory locality and performance
        private IPluginBase[] _plugins = Array.Empty<IPluginBase>();
        private PluginLoadContext[] _loadContexts = Array.Empty<PluginLoadContext>();
        private int _pluginCount;
        private readonly object _lock = new();
        private bool _disposed;
        #endregion

        #region Properties
        /// <summary>
        /// Gets loaded plugins with zero-allocation enumeration.
        /// </summary>
        public ReadOnlySpan<IPluginBase> Plugins
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _plugins.AsSpan(0, _pluginCount);
        }

        /// <summary>
        /// Gets plugin count with inlined access.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _pluginCount;
        }

        /// <summary>
        /// Gets the list of loaded plugins as a read-only collection (for compatibility).
        /// </summary>
        public IReadOnlyList<IPluginBase> PluginsList => _plugins.AsSpan(0, _pluginCount).ToArray();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManager"/> class.
        /// </summary>
        public PluginManager() { }
        #endregion

        #region Methods
        /// <summary>
        /// Loads plugins from the specified directory with optimized scanning and minimal allocations.
        /// </summary>
        /// <param name="pluginDirectory">The directory containing plugin assemblies (.dll files).</param>
        /// <exception cref="DirectoryNotFoundException">Thrown if the plugin directory does not exist.</exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void LoadPlugins(string pluginDirectory)
        {
            if (!Directory.Exists(pluginDirectory))
                throw new DirectoryNotFoundException($"Plugin directory not found: {pluginDirectory}");

            var dllFiles = Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories);

            foreach (var dllPath in dllFiles)
            {
                try
                {
                    LoadPluginAssembly(dllPath);
                }
                catch (Exception ex) when (ex is BadImageFormatException || ex is FileLoadException || ex is ReflectionTypeLoadException)
                {
                    // Continue with other plugins on failure - invalid or incompatible assemblies
                }
            }
        }

        /// <summary>
        /// Loads plugins from the specified directory asynchronously.
        /// </summary>
        /// <param name="pluginDirectory">The directory containing plugin assemblies (.dll files).</param>
        public Task LoadPluginsAsync(string pluginDirectory)
        {
            LoadPlugins(pluginDirectory);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Loads plugins from the specified directory and returns detailed results for each assembly.
        /// </summary>
        /// <param name="pluginDirectory">The directory containing plugin assemblies (.dll files).</param>
        /// <returns>A collection of load results for each assembly processed.</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown if the plugin directory does not exist.</exception>
        public IEnumerable<PluginLoadResult> LoadPluginsWithResults(string pluginDirectory)
        {
            if (!Directory.Exists(pluginDirectory))
                throw new DirectoryNotFoundException($"Plugin directory not found: {pluginDirectory}");

            var dllFiles = Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories);
            var results = new List<PluginLoadResult>();

            foreach (var dllPath in dllFiles)
            {
                results.Add(LoadPluginWithResult(dllPath));
            }

            return results;
        }

        /// <summary>
        /// Loads a single plugin assembly and returns the result.
        /// </summary>
        /// <param name="assemblyPath">The path to the plugin assembly.</param>
        /// <returns>The load result indicating success or failure.</returns>
        public PluginLoadResult LoadPluginWithResult(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                return new PluginLoadResult(assemblyPath, "Assembly file not found");
            }

            try
            {
                return LoadPluginAssemblyWithResult(assemblyPath);
            }
            catch (Exception ex) when (ex is BadImageFormatException || ex is FileLoadException || ex is ReflectionTypeLoadException)
            {
                return new PluginLoadResult(assemblyPath, "Failed to load assembly", ex);
            }
        }

        /// <summary>
        /// Initializes all loaded plugins with minimal overhead.
        /// </summary>
        /// <param name="context">The plugin host context to provide to plugins.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        public async Task InitializeAllPluginsAsync(IPluginHostContext? context = null, CancellationToken cancellationToken = default)
        {
            // Can't use span across await boundary, so iterate by index
            for (int i = 0; i < _pluginCount; i++)
            {
                var plugin = _plugins[i];
                try
                {
                    switch (plugin)
                    {
                        case IPluginAsync asyncPlugin:
                            await asyncPlugin.InitializeAsync(context, cancellationToken).ConfigureAwait(false);
                            break;
                        case IPlugin syncPlugin:
                            syncPlugin.Initialize(context);
                            break;
                    }
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException || ex is NotSupportedException)
                {
                    // Continue with other plugins on initialization failure
                }
            }
        }

        /// <summary>
        /// Stops all loaded plugins and unloads their contexts.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        public async Task StopAllPluginsAsync(CancellationToken cancellationToken = default)
        {
            // Can't use span across await boundary, so iterate by index
            for (int i = 0; i < _pluginCount; i++)
            {
                var plugin = _plugins[i];
                try
                {
                    switch (plugin)
                    {
                        case IPluginAsync asyncPlugin:
                            await asyncPlugin.OnStopAsync(cancellationToken).ConfigureAwait(false);
                            break;
                        case IPlugin syncPlugin:
                            syncPlugin.OnStop();
                            break;
                    }
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentException || ex is NotSupportedException)
                {
                    // Continue stopping other plugins even if one fails
                }
            }

            // Clean up contexts
            for (int i = 0; i < _pluginCount; i++)
            {
                try
                {
                    _loadContexts[i]?.Dispose();
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is ObjectDisposedException)
                {
                    // Continue cleanup even if disposal fails
                }
            }

            _pluginCount = 0;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads a single plugin assembly with optimized type checking.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LoadPluginAssembly(string assemblyPath)
        {
            PluginLoadContext? loadContext = null;
            try
            {
                loadContext = new PluginLoadContext(assemblyPath);
                var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);

                var pluginTypes = assembly.GetTypes()
                    .Where(type => !type.IsAbstract && type.IsClass &&
                                  (typeof(IPlugin).IsAssignableFrom(type) || typeof(IPluginAsync).IsAssignableFrom(type)));

                foreach (var pluginType in pluginTypes)
                {
                    if (Activator.CreateInstance(pluginType) is IPluginBase plugin && loadContext != null)
                    {
                        AddPlugin(plugin, loadContext);
                        loadContext = null; // Transfer ownership to AddPlugin method
                    }
                }
            }
            finally
            {
                // Only dispose if ownership wasn't transferred
                loadContext?.Dispose();
            }
        }

        /// <summary>
        /// Loads a single plugin assembly with optimized type checking and returns detailed result.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private PluginLoadResult LoadPluginAssemblyWithResult(string assemblyPath)
        {
            PluginLoadContext? loadContext = null;
            try
            {
                loadContext = new PluginLoadContext(assemblyPath);
                var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);

                var pluginTypes = assembly.GetTypes()
                    .Where(type => !type.IsAbstract && type.IsClass &&
                                  (typeof(IPlugin).IsAssignableFrom(type) || typeof(IPluginAsync).IsAssignableFrom(type)));

                var loadedPlugins = new List<IPluginBase>();

                foreach (var pluginType in pluginTypes)
                {
                    if (Activator.CreateInstance(pluginType) is IPluginBase plugin && loadContext != null)
                    {
                        AddPlugin(plugin, loadContext);
                        loadedPlugins.Add(plugin);
                        loadContext = null; // Transfer ownership to AddPlugin method
                    }
                }

                if (loadedPlugins.Count > 0)
                {
                    // Return the first plugin loaded (most common case)
                    return new PluginLoadResult(loadedPlugins[0], assemblyPath);
                }
                else
                {
                    return new PluginLoadResult(assemblyPath, "No valid plugin types found in assembly");
                }
            }
            catch (Exception ex) when (ex is BadImageFormatException || ex is FileLoadException || ex is ReflectionTypeLoadException || ex is InvalidOperationException)
            {
                return new PluginLoadResult(assemblyPath, "Failed to load or instantiate plugin", ex);
            }
            finally
            {
                // Only dispose if ownership wasn't transferred
                loadContext?.Dispose();
            }
        }

        /// <summary>
        /// Efficient plugin storage with automatic array resizing.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddPlugin(IPluginBase plugin, PluginLoadContext context)
        {
            lock (_lock)
            {
                if (_pluginCount >= _plugins.Length)
                {
                    var newSize = _plugins.Length == 0 ? 4 : _plugins.Length * 2;
                    Array.Resize(ref _plugins, newSize);
                    Array.Resize(ref _loadContexts, newSize);
                }

                _plugins[_pluginCount] = plugin;
                _loadContexts[_pluginCount] = context;
                _pluginCount++;
            }
        }
        #endregion

        #region IDisposable Implementation
        /// <summary>
        /// Disposes the plugin manager and all loaded plugins.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            StopAllPluginsAsync().GetAwaiter().GetResult();

            _plugins = Array.Empty<IPluginBase>();
            _loadContexts = Array.Empty<PluginLoadContext>();
            _disposed = true;

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
