using System;

namespace Navz.PluginEngine.Abstractions
{
    /// <summary>
    /// Provides common metadata properties for all plugins in the Navz Plugin Engine.
    /// </summary>
    public interface IPluginBase
    {
        /// <summary>
        /// Gets the unique identifier of the plugin instance.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the display name of the plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the semantic version of the plugin.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets a brief description of the plugin's functionality.
        /// </summary>
        string Description { get; }
    }
}
