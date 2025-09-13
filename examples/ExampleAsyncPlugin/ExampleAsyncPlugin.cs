using System;
using System.Threading;
using System.Threading.Tasks;
using Navz.PluginEngine.Abstractions;

namespace ExampleAsyncPlugin
{
    /// <summary>
    /// Example async plugin implementation for testing the plugin engine.
    /// </summary>
    public class ExampleAsyncPlugin : IPluginAsync
    {
        /// <inheritdoc/>
        public string Id => "navz.example.async.plugin";
        /// <inheritdoc/>
        public string Name => "Example Async Plugin";
        /// <inheritdoc/>
        public string Version => "1.0.0";
        /// <inheritdoc/>
        public string Description => "An example async plugin for testing purposes";

        /// <summary>
        /// Asynchronously initializes the plugin.
        /// </summary>
        /// <param name="context">The plugin host context.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the initialization operation.</returns>
        public async Task InitializeAsync(IPluginHostContext? context = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var logger = context?.GetService<object>() as dynamic;
                logger?.LogInfo($"{Name} initializing (async)...");
                if (cancellationToken.IsCancellationRequested)
                {
                    logger?.LogInfo($"{Name} initialization cancelled.");
                    return;
                }
                await Task.Delay(100, cancellationToken);
                logger?.LogInfo($"{Name} (Async) initialized successfully!");
                Console.WriteLine($"{Name} (Async) initialized successfully!");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"{Name} initialization cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Plugin Error] {Name} failed to initialize: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously stops the plugin and performs cleanup.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the stop operation.</returns>
        public async Task OnStopAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"{Name} stop cancelled.");
                    return;
                }
                await Task.Delay(50, cancellationToken);
                Console.WriteLine($"{Name} (Async) stopped successfully!");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"{Name} stop cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Plugin Error] {Name} failed to stop: {ex.Message}");
            }
        }
    }
}
