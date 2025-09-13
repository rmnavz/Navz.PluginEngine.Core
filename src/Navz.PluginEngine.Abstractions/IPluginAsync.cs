using System;
using System.Threading;
using System.Threading.Tasks;

namespace Navz.PluginEngine.Abstractions
{
    /// <summary>
    /// Defines the contract for plugins supporting asynchronous operations in the Navz Plugin Engine.
    /// Provides async initialization and cleanup methods, allowing plugins to perform non-blocking setup and teardown.
    /// </summary>
    public interface IPluginAsync : IPluginBase
    {
        /// <summary>
        /// Asynchronously initializes the plugin instance. Called by the host during plugin startup.
        /// </summary>
        /// <param name="context">Optional context providing access to host services and environment information.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// Use this method to perform non-blocking setup, such as I/O operations or service registration.
        /// </remarks>
        Task InitializeAsync(IPluginHostContext? context = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously stops the plugin and performs cleanup. Called by the host during shutdown or plugin unload.
        /// </summary>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous stop operation.</returns>
        /// <remarks>
        /// Implementations should release resources and perform any necessary asynchronous teardown.
        /// </remarks>
        Task OnStopAsync(CancellationToken cancellationToken = default);
    }
}
