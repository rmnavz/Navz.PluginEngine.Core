using System;

namespace Navz.PluginEngine.Abstractions
{
    /// <summary>
    /// Defines the fundamental contract for all plugins in the Navz Plugin Engine.
    /// Plugins implementing this interface are managed by the host and must provide initialization and cleanup logic.
    /// </summary>
    public interface IPlugin : IPluginBase
    {
        /// <summary>
        /// Initializes the plugin instance. Called by the host during plugin startup.
        /// </summary>
        /// <param name="context">Optional context providing access to host services and environment information.</param>
        /// <remarks>
        /// This method should be used to allocate resources, register services, or perform setup required for plugin operation.
        /// </remarks>
        void Initialize(IPluginHostContext? context = null);

        /// <summary>
        /// Stops the plugin and performs cleanup. Called by the host during shutdown or plugin unload.
        /// </summary>
        /// <remarks>
        /// Implementations should release resources, unregister services, and perform any necessary teardown.
        /// </remarks>
        void OnStop();
    }
}
