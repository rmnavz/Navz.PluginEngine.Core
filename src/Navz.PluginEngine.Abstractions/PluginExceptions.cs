using System;

namespace Navz.PluginEngine.Abstractions
{
    /// <summary>
    /// Base exception for plugin-related operations.
    /// </summary>
    public abstract class PluginException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        protected PluginException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        protected PluginException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Gets or sets the plugin ID associated with this exception.
        /// </summary>
        public string? PluginId { get; set; }

        /// <summary>
        /// Gets or sets the assembly path associated with this exception.
        /// </summary>
        public string? AssemblyPath { get; set; }
    }

    /// <summary>
    /// Exception thrown when a plugin fails to load.
    /// </summary>
    public class PluginLoadException : PluginException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public PluginLoadException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PluginLoadException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadException"/> class.
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly that failed to load.</param>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PluginLoadException(string assemblyPath, string message, Exception innerException)
            : base(message, innerException)
        {
            AssemblyPath = assemblyPath;
        }
    }

    /// <summary>
    /// Exception thrown when a plugin fails to initialize.
    /// </summary>
    public class PluginInitializationException : PluginException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginInitializationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public PluginInitializationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginInitializationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PluginInitializationException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginInitializationException"/> class.
        /// </summary>
        /// <param name="pluginId">The ID of the plugin that failed to initialize.</param>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PluginInitializationException(string pluginId, string message, Exception innerException)
            : base(message, innerException)
        {
            PluginId = pluginId;
        }
    }

    /// <summary>
    /// Exception thrown when a plugin operation is invalid or not supported.
    /// </summary>
    public class PluginOperationException : PluginException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginOperationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public PluginOperationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginOperationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PluginOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
