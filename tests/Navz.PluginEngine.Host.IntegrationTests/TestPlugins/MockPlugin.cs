using System;
using System.Threading;
using System.Threading.Tasks;
using Navz.PluginEngine.Abstractions;

namespace Navz.PluginEngine.Host.IntegrationTests.TestPlugins
{
    /// <summary>
    /// A mock plugin for integration testing.
    /// </summary>
    public class MockPlugin : IPlugin
    {
        /// <summary>
        /// Gets the plugin ID.
        /// </summary>
        public string Id => "MockPlugin";
        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public string Name => "Mock Plugin";
        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public string Version => "1.0.0";
        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        public string Description => "A mock plugin for integration testing";

        private IPluginHostContext? _context;

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        /// <param name="context">The plugin host context.</param>
        public void Initialize(IPluginHostContext? context = null)
        {
            _context = context;
            // Initialization logic
        }

        /// <summary>
        /// Stops the plugin.
        /// </summary>
        public void OnStop()
        {
            // Stopping logic
            _context = null;
        }
    }
}
