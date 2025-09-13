using System;
using System.Collections.Generic;

namespace Navz.PluginEngine.Host
{
    /// <summary>
    /// Represents metadata information about a discovered plugin.
    /// </summary>
    public class PluginMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginMetadata"/> class.
        /// </summary>
        /// <param name="name">The plugin name.</param>
        /// <param name="version">The plugin version.</param>
        /// <param name="assemblyPath">The path to the plugin assembly.</param>
        /// <param name="isAsync">Indicates whether the plugin supports async operations.</param>
        /// <param name="pluginTypes">The list of plugin type names in the assembly.</param>
        public PluginMetadata(string name, string version, string assemblyPath, bool isAsync, IReadOnlyList<string> pluginTypes)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            AssemblyPath = assemblyPath ?? throw new ArgumentNullException(nameof(assemblyPath));
            IsAsync = isAsync;
            PluginTypes = pluginTypes ?? throw new ArgumentNullException(nameof(pluginTypes));

            // Set file metadata
            if (System.IO.File.Exists(assemblyPath))
            {
                var fileInfo = new System.IO.FileInfo(assemblyPath);
                FileSizeBytes = fileInfo.Length;
                LastModified = fileInfo.LastWriteTime;
            }
        }

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the path to the plugin assembly.
        /// </summary>
        public string AssemblyPath { get; }

        /// <summary>
        /// Gets a value indicating whether the plugin supports async operations.
        /// </summary>
        public bool IsAsync { get; }

        /// <summary>
        /// Gets the list of plugin type names in the assembly.
        /// </summary>
        public IReadOnlyList<string> PluginTypes { get; }

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        public string? Author { get; init; }

        /// <summary>
        /// Gets the unique identifier of the plugin.
        /// </summary>
        public string? PluginId { get; init; }

        /// <summary>
        /// Gets the file size in bytes.
        /// </summary>
        public long FileSizeBytes { get; }

        /// <summary>
        /// Gets the last modified date of the plugin assembly.
        /// </summary>
        public DateTime LastModified { get; }
    }
}
