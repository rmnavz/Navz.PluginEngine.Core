using System;
using System.Threading;
using System.Threading.Tasks;
using Navz.PluginEngine.Abstractions;

namespace Navz.PluginEngine.Host.IntegrationTests.TestPlugins
{
    /// <summary>
    /// A mock async plugin for integration testing.
    /// </summary>
    public class MockAsyncPlugin : IPluginAsync
    {
        /// <summary>
        /// Gets the plugin ID.
        /// </summary>
        public string Id => "MockAsyncPlugin";
        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public string Name => "Mock Async Plugin";
        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public string Version => "1.0.0";
        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public string Description => "A mock async plugin for integration testing";

        private IPluginHostContext? _context;

        /// <summary>
        /// Initializes the plugin asynchronously.
        /// </summary>
        /// <param name="context">The plugin host context.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        public async Task InitializeAsync(IPluginHostContext? context = null, CancellationToken cancellationToken = default)
        {
            await Task.Delay(100, cancellationToken); // Simulate async work
            _context = context;
            // Initialization logic
        }

        /// <summary>
        /// Stops the plugin asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        public async Task OnStopAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(100, cancellationToken); // Simulate async work
            // Stopping logic
            _context = null;
        }
    }
}
