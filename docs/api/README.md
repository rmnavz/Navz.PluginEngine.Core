# API Documentation

## Core Packages

### Navz.PluginEngine.Abstractions

This package contains the core interfaces and base types for the plugin system.

#### Interfaces

1. `IPluginBase`
   - Base interface for all plugins
   - Properties:
     - `Id`: Unique plugin identifier
     - `Name`: Display name
     - `Version`: Semantic version
     - `Description`: Plugin functionality description

2. `IPlugin`
   - Synchronous plugin interface
   - Methods:
     - `Initialize(IPluginHostContext?)`: Setup resources
     - `OnStop()`: Cleanup resources

3. `IPluginAsync`
   - Asynchronous plugin interface
   - Methods:
     - `InitializeAsync(IPluginHostContext?, CancellationToken)`: Async setup
     - `OnStopAsync(CancellationToken)`: Async cleanup

4. `IPluginHostContext`
   - Basic host-plugin communication
   - Properties:
     - `HostName`: Host application name
     - `HostVersion`: Host application version
   - Methods:
     - `GetService<T>()`: Basic service resolution
     - `GetService(Type)`: Non-generic resolution

### Navz.PluginEngine.Host

This package contains the core implementation for plugin loading and lifecycle management.

#### Classes

1. `PluginManager`
   - Memory-optimized plugin storage using arrays
   - Methods:
     - `LoadPlugins(string)`: Load plugins from directory
     - `LoadPluginsAsync(string)`: Async plugin loading
     - `InitializeAllPluginsAsync(IPluginHostContext?, CancellationToken)`: Initialize plugins
     - `StopAllPluginsAsync(CancellationToken)`: Stop and unload plugins
   - Properties:
     - `Plugins`: Zero-allocation plugin enumeration
     - `Count`: Current plugin count
     - `PluginsList`: Read-only plugin list

2. `PluginLoadContext`
   - Custom AssemblyLoadContext for plugin isolation
   - Basic assembly loading and unloading
   - Clean plugin unloading support

3. `DefaultPluginHostContext`
   - Simple IPluginHostContext implementation
   - Basic host information provision
   - Simple service resolution

## Detailed API Reference

For detailed API documentation, including all public members, properties, and methods, please refer to the XML documentation in the source code or use the generated API documentation in your IDE.

Note: This is a placeholder for the complete API documentation. The full documentation will be generated automatically from XML comments in the source code.
