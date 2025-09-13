using System;

namespace Navz.PluginEngine.Abstractions
{
    /// <summary>
    /// Provides plugins with access to host services, environment information, and dependency resolution in the Navz Plugin Engine.
    /// </summary>
    public interface IPluginHostContext
    {
        /// <summary>
        /// Gets the name of the host application managing the plugins.
        /// </summary>
        string HostName { get; }

        /// <summary>
        /// Gets the version of the host application.
        /// </summary>
        string HostVersion { get; }

        /// <summary>
        /// Resolves a service of the specified type from the host environment.
        /// </summary>
        /// <typeparam name="T">The type of the service to resolve.</typeparam>
        /// <returns>The resolved service instance, or null if not found.</returns>
        T? GetService<T>() where T : class;

        /// <summary>
        /// Resolves a service of the specified type from the host environment (non-generic overload).
        /// </summary>
        /// <param name="serviceType">The type of the service to resolve.</param>
        /// <returns>The resolved service instance, or null if not found.</returns>
        object? GetService(Type serviceType);

        /// <summary>
        /// Gets a configuration value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the configuration value.</typeparam>
        /// <param name="key">The configuration key.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The configuration value or the default value if not found.</returns>
        T GetConfiguration<T>(string key, T defaultValue = default!);

        /// <summary>
        /// Checks if a configuration key exists.
        /// </summary>
        /// <param name="key">The configuration key to check.</param>
        /// <returns>True if the key exists, false otherwise.</returns>
        bool HasConfiguration(string key);

        /// <summary>
        /// Logs an error message with optional exception details.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="exception">The optional exception associated with the error.</param>
        void LogError(string message, Exception? exception = null);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        void LogInformation(string message);
    }
}
