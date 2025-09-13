# Getting Started with Navz Plugin Engine

This guide will help you get started with using the Navz Plugin Engine in your .NET applications.

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, Visual Studio Code, or JetBrains Rider
- Basic understanding of .NET and C#

## Installation

Add the NuGet packages to your project:

```bash
# For plugin development
dotnet add package Navz.PluginEngine.Abstractions

# For host application development
dotnet add package Navz.PluginEngine.Host
```

## Quick Start Guide

### Creating a Plugin

1. Create a new Class Library project:

    ```bash
    dotnet new classlib -n MyFirstPlugin
    ```

2. Add the Abstractions package:

    ```bash
    dotnet add package Navz.PluginEngine.Abstractions
    ```

3. Implement the plugin interface:

    ```csharp
    using Navz.PluginEngine.Abstractions;

    public class MyPlugin : IPlugin
    {
        public string Name => "MyFirstPlugin";
        public string Version => "1.0.0";

        public void Initialize(IPluginHostContext? context = null)
        {
            // Plugin initialization code
        }

        public void OnStop()
        {
            // Cleanup code
        }
    }
    ```

### Creating a Host Application

1. Create a new Console Application:

    ```bash
    dotnet new console -n MyHostApp
    ```

2. Add the Host package:

    ```bash
    dotnet add package Navz.PluginEngine.Host
    ```

3. Initialize the plugin manager:

    ```csharp
    using Navz.PluginEngine.Host;

    var pluginManager = new PluginManager();
    var hostContext = new DefaultPluginHostContext();

    // Load plugins from a directory
    await pluginManager.LoadPluginsAsync("./plugins", hostContext);

    // Start all plugins
    await pluginManager.StartAllPluginsAsync();
    ```

## Next Steps

- Check out the [Example Projects](../../examples/) for complete working examples
- Read the [Architecture Overview](../architecture/overview.md) for deeper understanding
- Learn about [Plugin Isolation](../architecture/plugin-isolation.md) for security and stability
- See the [Best Practices Guide](best-practices.md) for recommendations
