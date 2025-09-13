using Navz.PluginEngine.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExamplePlugin
{
    /// <summary>
    /// Example plugin implementation for testing the plugin engine.
    /// </summary>
    public class ExamplePlugin : IPlugin
    {
        /// <inheritdoc/>
        public string Id => "navz.example.plugin";
        /// <inheritdoc/>
        public string Name => "ExamplePlugin";
        /// <inheritdoc/>
        public string Version => "1.0.0";
        /// <inheritdoc/>
        public string Description => "An example plugin demonstrating the Navz Plugin Engine functionality.";

        /// <summary>
        /// Called when the plugin is initialized by the host.
        /// </summary>
        /// <param name="context">The host application context.</param>
        public void Initialize(IPluginHostContext? context = null)
        {
            try
            {
                // Try to use host logger if available
                var logger = context?.GetService<object>() as dynamic;
                logger?.LogInfo($"{Name} v{Version} initializing...");
                Console.WriteLine($"{Name} v{Version} initialized.");
                if (context != null)
                {
                    Console.WriteLine($"Host: {context.HostName} v{context.HostVersion}");
                    // Example: retrieve a service
                    var someService = context.GetService<object>();
                    if (someService != null)
                    {
                        Console.WriteLine($"Retrieved service: {someService.GetType().Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Plugin Error] {Name} failed to initialize: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when the plugin is being stopped or unloaded.
        /// </summary>
        public void OnStop()
        {
            try
            {
                Console.WriteLine($"{Name} stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Plugin Error] {Name} failed to stop: {ex.Message}");
            }
        }
    }
}
