namespace Navz.PluginEngine.Abstractions
{
    /// <summary>
    /// Represents the result of a plugin loading operation.
    /// </summary>
    public class PluginLoadResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadResult"/> class for a successful load.
        /// </summary>
        /// <param name="plugin">The successfully loaded plugin.</param>
        /// <param name="assemblyPath">The path to the plugin assembly.</param>
        public PluginLoadResult(IPluginBase plugin, string assemblyPath)
        {
            Success = true;
            Plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
            AssemblyPath = assemblyPath ?? throw new ArgumentNullException(nameof(assemblyPath));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadResult"/> class for a failed load.
        /// </summary>
        /// <param name="assemblyPath">The path to the plugin assembly that failed to load.</param>
        /// <param name="errorMessage">The error message describing the failure.</param>
        /// <param name="exception">The exception that caused the failure, if any.</param>
        public PluginLoadResult(string assemblyPath, string errorMessage, Exception? exception = null)
        {
            Success = false;
            AssemblyPath = assemblyPath ?? throw new ArgumentNullException(nameof(assemblyPath));
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
            Exception = exception;
        }

        /// <summary>
        /// Gets a value indicating whether the plugin was loaded successfully.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets the loaded plugin instance if the operation was successful.
        /// </summary>
        public IPluginBase? Plugin { get; }

        /// <summary>
        /// Gets the path to the plugin assembly.
        /// </summary>
        public string AssemblyPath { get; }

        /// <summary>
        /// Gets the error message if the operation failed.
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// Gets the exception that caused the failure, if any.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Gets the plugin type name if available.
        /// </summary>
        public string? PluginTypeName => Plugin?.GetType().Name;
    }
}
