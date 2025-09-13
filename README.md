# Navz Plugin Engine Core

[![.NET Core CI](https://github.com/rmnavz/Navz.PluginEngine.Core/actions/workflows/ci.yml/badge.svg)](https://github.com/rmnavz/Navz.PluginEngine.Core/actions/workflows/ci.yml)
[![Continuous Deployment](https://github.com/rmnavz/Navz.PluginEngine.Core/actions/workflows/cd.yml/badge.svg)](https://github.com/rmnavz/Navz.PluginEngine.Core/actions/workflows/cd.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Navz.PluginEngine.Abstractions.svg)](https://www.nuget.org/packages/Navz.PluginEngine.Abstractions/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A lightweight, memory-optimized plugin engine for .NET applications providing assembly isolation and basic lifecycle management. Built for .NET 8.0+ with focus on performance and reliability.

## 🚀 Key Features

- **� Memory Optimization**: Zero-allocation enumeration using Span with array-based storage
- **🔒 Assembly Isolation**: Per-plugin AssemblyLoadContext with clean unloading
- **⚡ Thread Safety**: Thread-safe plugin operations with optimized locking
- **� Async Support**: Both synchronous and asynchronous plugin interfaces
- **�️ Error Handling**: Graceful plugin failure handling and cleanup
- **� NuGet Ready**: Easy integration via NuGet packages

## 📦 NuGet Packages

| Package | Purpose | NuGet |
|---------|---------|-------|
| `Navz.PluginEngine.Abstractions` | Core interfaces and metadata | [![NuGet](https://img.shields.io/nuget/v/Navz.PluginEngine.Abstractions.svg)](https://www.nuget.org/packages/Navz.PluginEngine.Abstractions/) |
| `Navz.PluginEngine.Host` | Memory-optimized plugin management | [![NuGet](https://img.shields.io/nuget/v/Navz.PluginEngine.Host.svg)](https://www.nuget.org/packages/Navz.PluginEngine.Host/) |

## 🏗️ Architecture Overview

```text
┌─────────────────────────────────────────────────────────────┐
│                    HOST APPLICATION                         │
│  ┌─────────────────────────────────────────────────────────┐│
│  │              Your Application                           ││
│  │  • Plugin Loading & Management                          ││
│  │  • Optional Host Context                                ││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                 PLUGIN ENGINE                               │
│  ┌─────────────────┐    ┌─────────────────────────────────┐ │
│  │ PluginManager   │    │   PluginLoadContext            │ │
│  │ • Array Storage │    │   • Assembly Isolation         │ │
│  │ • Basic Loading │    │   • Clean Unloading           │ │
│  │ • Lifecycle     │    │   • Memory Management         │ │
│  └─────────────────┘    └─────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                      PLUGINS                               │
│  ┌─────────────────┐    ┌─────────────────┐                │
│  │   Plugin A      │    │   Plugin B      │    ...         │
│  │   (Sync/Async)  │    │   (Sync/Async)  │                │
│  └─────────────────┘    └─────────────────┘                │
└─────────────────────────────────────────────────────────────┘
```

## 🚀 Quick Start

## 🛠️ Deployment

### Build & Package

- Build the solution:

    ```bash
    dotnet build Navz.PluginEngine.Core.sln -c Release
    ```

- Create NuGet packages (from `src/`):

    ```bash
    dotnet pack Navz.PluginEngine.Abstractions/ -c Release
    dotnet pack Navz.PluginEngine.Host/ -c Release
    ```

- Packages will be in `src/<Project>/bin/Release/` as `.nupkg` files.
- Publish to NuGet or your private feed:

    ```bash
    dotnet nuget push src/Navz.PluginEngine.Abstractions/bin/Release/*.nupkg --api-key <key> --source <source>
    dotnet nuget push src/Navz.PluginEngine.Host/bin/Release/*.nupkg --api-key <key> --source <source>
    ```

### Deploying Plugins

- Place plugin DLLs in a dedicated directory (e.g., `Plugins/` or `plugins/`).
- The host app should load plugins from this directory at runtime.
- For self-contained deployment, ensure all plugin dependencies are present.

### CI/CD

- Use the provided GitHub Actions workflows (`.github/workflows/ci.yml`, `cd.yml`) for automated build, test, and deployment.
- Customize these workflows for your environment as needed.

## ⚙️ Configuration

### Plugin Directory

- Set the directory path in your host app:

    ```csharp
    var pluginDir = Path.Combine(AppContext.BaseDirectory, "plugins");
    manager.LoadPlugins(pluginDir);
    ```

- You can use environment variables or appsettings.json to make this configurable.

### Host Context & Services

- Implement `IPluginHostContext` to provide services/configuration to plugins.
- Register services and configuration values before initializing plugins.

    ```csharp
    var context = new DefaultPluginHostContext("MyHost", "1.0.0");
    context.RegisterService<IMyService>(new MyService());
    context.RegisterConfiguration("SomeKey", "SomeValue");
    await manager.InitializeAllPluginsAsync(context);
    ```

### Environment Variables

- Plugins can read environment variables via `Environment.GetEnvironmentVariable`.
- Use for secrets, connection strings, or deployment-specific settings.

## 📝 Usage in Production

### Best Practices

- Always use `async` plugin interfaces for I/O-bound plugins.
- Use structured logging (integrate Serilog/NLog in your host for production logs).
- Validate plugin assemblies before loading (signature, version, etc.).
- Run plugins in isolated directories for security.
- Monitor memory usage and unload plugins when not needed.

### Example: Production-Ready Host

```csharp
var manager = new PluginManager();
var pluginDir = Environment.GetEnvironmentVariable("PLUGIN_DIR") ?? "plugins";
manager.LoadPlugins(pluginDir);
var context = new DefaultPluginHostContext("MyHost", "1.0.0");
// Register production services/configuration
await manager.InitializeAllPluginsAsync(context);
// ...
await manager.StopAllPluginsAsync();
```

### 1. Install Packages

```bash
# For plugin development
dotnet add package Navz.PluginEngine.Abstractions

# For host applications
dotnet add package Navz.PluginEngine.Host
```

### 2. Create a Plugin

```csharp
using Navz.PluginEngine.Abstractions;

public class MyPlugin : IPlugin
{
    public string Id => "my-plugin";
    public string Name => "My Plugin";
    public string Version => "1.0.0";
    public string Description => "A sample plugin";

    public Task Initialize(IPluginHostContext? context)
    {
        // Plugin initialization logic
        Console.WriteLine($"Plugin {Name} initialized!");
        return Task.CompletedTask;
    }

    public Task OnStop()
    {
        // Plugin cleanup logic
        Console.WriteLine($"Plugin {Name} stopped!");
        return Task.CompletedTask;
    }
}
```

### 3. Load Plugins in Host

```csharp
using Navz.PluginEngine.Host;

var manager = new PluginManager();

// Load plugins - each gets its own AssemblyLoadContext
await manager.LoadPluginsAsync("plugins/");

// Initialize plugins with optional host context
await manager.InitializeAllPluginsAsync(hostContext);

// Access plugins with zero-allocation enumeration
foreach (var plugin in manager.Plugins)
{
    Console.WriteLine($"Loaded: {plugin.Name} v{plugin.Version}");
}

// Later: Stop and unload plugins
await manager.StopAllPluginsAsync();
```

## 📁 Repository Structure

```text
Navz.PluginEngine.Core/
├── 📂 src/                         # 🏗️ Core Libraries
│   ├── � Navz.PluginEngine.Abstractions/
│   │   ├── � IPlugin.cs          # Sync plugin interface
│   │   ├── 📄 IPluginAsync.cs     # Async plugin interface
│   │   └── 📄 IPluginBase.cs      # Common plugin metadata
│   └── 📂 Navz.PluginEngine.Host/
│       ├── 📄 PluginManager.cs     # Memory-optimized manager
│       └── � PluginLoadContext.cs # Assembly isolation
├── 📂 examples/                     # 🎯 Example Code
│   ├── 📂 ExamplePlugin/          # Sync plugin example
│   ├── 📂 ExampleAsyncPlugin/     # Async plugin sample
│   └── 📂 ExampleHostApp/         # Host usage example
├── 📂 tests/                       # 🧪 Test Projects
│   ├── � Host.UnitTests/         # Unit test coverage
│   └── � Host.IntegrationTests/  # Integration testing
└── 📄 Navz.PluginEngine.Core.sln
```

## 🧩 Core Components

### PluginManager

Memory-optimized plugin management:

- **Storage**: Array-based with automatic resizing
- **Access**: Zero-allocation enumeration via Span
- **Thread Safety**: Protected operations with minimal locking
- **Cleanup**: Proper disposal and unloading

### PluginLoadContext

Assembly isolation implementation:

- **Isolation**: Per-plugin AssemblyLoadContext
- **Memory**: Clean unloading and cache cleanup
- **Loading**: Basic assembly resolution
- **Disposal**: Automatic context cleanup

### Plugin Interfaces

Minimal plugin contracts:

- **IPluginBase**: Core metadata (Id, Name, Version)
- **IPlugin**: Sync Initialize/Stop methods
- **IPluginAsync**: Async Initialize/Stop methods
- **IPluginHostContext**: Basic service resolution

## 📚 Documentation

- 📖 **[Architecture Overview](docs/architectural-overview.md)**: Deep dive into design decisions
- 🤝 **[Contribution Guide](docs/contribution-guide.md)**: How to contribute to the project
- 🔄 **[Workflow Documentation](.github/workflows/README.md)**: CI/CD pipeline details
- 🎯 **[Examples](examples/)**: Working code examples and templates

## 🧪 Testing

Run the complete test suite:

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/Navz.PluginEngine.Host.Tests/
```

## 🤝 Contributing

We welcome contributions! Please see our [Contribution Guide](docs/contribution-guide.md) for details on:

- 📋 Code standards and conventions
- 🔄 Development workflow and branching strategy
- 🧪 Testing requirements and guidelines
- 📝 Documentation standards

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Links

- 📦 **NuGet Packages**: [Navz.PluginEngine.Abstractions](https://www.nuget.org/packages/Navz.PluginEngine.Abstractions/) | [Navz.PluginEngine.Host](https://www.nuget.org/packages/Navz.PluginEngine.Host/)
- 📚 **Documentation**: [https://rmnavz.github.io/Navz.PluginEngine.Core](https://rmnavz.github.io/Navz.PluginEngine.Core)
- 🐛 **Issues**: [GitHub Issues](https://github.com/rmnavz/Navz.PluginEngine.Core/issues)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/rmnavz/Navz.PluginEngine.Core/discussions)

---

## Table of Contents

- [Navz Plugin Engine Core](#navz-plugin-engine-core)
  - [🚀 Key Features](#-key-features)
  - [📦 NuGet Packages](#-nuget-packages)
  - [🏗️ Architecture Overview](#️-architecture-overview)
  - [🚀 Quick Start](#-quick-start)
  - [🛠️ Deployment](#️-deployment)
    - [Build \& Package](#build--package)
    - [Deploying Plugins](#deploying-plugins)
    - [CI/CD](#cicd)
  - [⚙️ Configuration](#️-configuration)
    - [Plugin Directory](#plugin-directory)
    - [Host Context \& Services](#host-context--services)
    - [Environment Variables](#environment-variables)
  - [📝 Usage in Production](#-usage-in-production)
    - [Best Practices](#best-practices)
    - [Example: Production-Ready Host](#example-production-ready-host)
    - [1. Install Packages](#1-install-packages)
    - [2. Create a Plugin](#2-create-a-plugin)
    - [3. Load Plugins in Host](#3-load-plugins-in-host)
  - [📁 Repository Structure](#-repository-structure)
  - [🧩 Core Components](#-core-components)
    - [PluginManager](#pluginmanager)
    - [PluginLoadContext](#pluginloadcontext)
    - [Plugin Interfaces](#plugin-interfaces)
  - [📚 Documentation](#-documentation)
  - [🧪 Testing](#-testing)
  - [🤝 Contributing](#-contributing)
  - [📄 License](#-license)
  - [🔗 Links](#-links)
  - [Table of Contents](#table-of-contents)
  - [Repository Structure](#repository-structure)
  - [Purpose](#purpose)
  - [Projects](#projects)
  - [Published NuGet Packages](#published-nuget-packages)
  - [Features](#features)
  - [Key Concepts](#key-concepts)
  - [Usage Example](#usage-example)
    - [ExampleHostApp/Program.cs](#examplehostappprogramcs)
    - [Async Plugin with Error Handling](#async-plugin-with-error-handling)
    - [Host Implementation](#host-implementation)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Basic Setup](#basic-setup)
  - [Sample Projects](#sample-projects)
  - [Testing](#testing)
  - [Contributing](#contributing)
  - [License](#license)
  - [Common Issues](#common-issues)
    - [Plugin Loading](#plugin-loading)
    - [Memory Usage](#memory-usage)
    - [Thread Safety](#thread-safety)
  - [Troubleshooting](#troubleshooting)

## Repository Structure

```text
Navz.PluginEngine.Core/
│
├── .github/                  # GitHub workflows and templates
│   ├── workflows/            # CI/CD definitions (e.g., ci.yml)
│   ├── ISSUE_TEMPLATE.md
│   └── PULL_REQUEST_TEMPLATE.md
│
├── docs/                     # Documentation (see docs/README.md for details)
│   ├── architectural-overview.md
│   ├── contribution-guide.md
│   └── README.md
│
├── src/                      # Core source code
│   ├── Navz.PluginEngine.Abstractions/
│   │   ├── Navz.PluginEngine.Abstractions.csproj
│   │   ├── IPlugin.cs
│   │   ├── IPluginAsync.cs
│   │   ├── IPluginBase.cs
│   │   └── IPluginHostContext.cs
│   │
│   └── Navz.PluginEngine.Host/
│       ├── Navz.PluginEngine.Host.csproj
│       ├── PluginManager.cs
│       └── PluginLoadContext.cs
│
├── examples/                  # Example plugins and host applications
│   ├── ExamplePlugin/
│   │   ├── ExamplePlugin.csproj
│   │   └── ExamplePlugin.cs
│   ├── ExampleAsyncPlugin/
│   │   ├── ExampleAsyncPlugin.csproj
│   │   └── ExampleAsyncPlugin.cs
│   └── ExampleHostApp/
│       ├── ExampleHostApp.csproj
│       └── Program.cs
│
├── tests/                    # Unit and integration tests
│   ├── Navz.PluginEngine.Host.Tests/
│   │   └── Navz.PluginEngine.Host.Tests.csproj
│   └── Navz.PluginEngine.Tests.Common/ (optional shared test utilities)
│
├── .editorconfig
├── .gitignore
├── Directory.Build.props
├── LICENSE
├── PluginEngine.Core.sln
└── README.md
```

<!-- Badges: Add build status, NuGet, license, etc. here -->

See each folder's README (where present) for more details.

## Purpose

The `Navz Plugin Engine Core` serves as the bedrock for building extensible .NET applications. It defines the minimal contract necessary for any plugin to interact with a host application and provides the fundamental mechanisms for the host to load these plugins in an isolated manner. This allows for modular application development, enabling feature additions, customizations, and third-party integrations without recompiling the main application.

## Projects

This repository comprises two core projects:

1. **`Navz.PluginEngine.Abstractions`**
   - **Role:** Defines the absolute minimum set of interfaces that constitute the "plugin contract." This includes the base `IPlugin` interface and the `IPluginHostContext` which provides plugins access to host services.
   - **Nature:** This project is designed to be extremely stable. Changes here are considered breaking changes for the entire plugin ecosystem.
   - **Target:** Referenced by `Navz.PluginEngine.Host`, `Navz.PluginEngine.CommonTypes`, and all individual plugin projects.

2. **`Navz.PluginEngine.Host`**
   - **Role:** Implements the core logic for the plugin engine on the host side. This includes the `PluginLoadContext` for assembly isolation and the `PluginManager` for discovering, loading, and managing plugin instances.
   - **Nature:** This project provides the runtime capabilities for the host application.
   - **Target:** Referenced by your main host application (e.g., `ExampleHostApp`).

## Published NuGet Packages

This repository publishes the following NuGet packages to your configured NuGet feed (e.g., Azure DevOps Artifacts, GitHub Packages, private feed):

- `Navz.PluginEngine.Abstractions`
  - **Description:** Contains core interfaces and contracts for the plugin engine.
  - **Usage:** Referenced by any project that wants to *define* a plugin, *implement* a plugin, or *interact with the core abstractions* of the plugin system (e.g., `Navz.PluginEngine.CommonTypes`).

- `Navz.PluginEngine.Host`
  - **Description:** Provides the runtime host-side implementation for discovering and loading plugins.
  - **Usage:** Referenced primarily by the main host application (e.g., `ExampleHostApp`) to enable plugin functionality. Automatically pulls `Navz.PluginEngine.Abstractions` as a dependency.

## Features

- **Assembly Isolation**: Each plugin runs in its own `AssemblyLoadContext`, preventing dependency conflicts
- **Dual Plugin Support**:
  - Synchronous plugins via `IPlugin` for simple operations
  - Asynchronous plugins via `IPluginAsync` for I/O-bound tasks
- **Service Resolution**: Plugin-host communication through `IPluginHostContext`
- **Safe Unloading**: Proper cleanup and unloading of plugin assemblies
- **Minimal Dependencies**: Core abstractions have no external dependencies
- **Modern Design**: Built for .NET 8.0+ with full async support

## Key Concepts

- **`IPluginBase`**: Common metadata interface providing ID, name, version, and description
- **`IPlugin`**: Synchronous plugin interface with Initialize and OnStop methods
- **`IPluginAsync`**: Asynchronous plugin interface with InitializeAsync and OnStopAsync methods
- **`IPluginHostContext`**: Service provider interface for host-plugin communication
- **`PluginManager`**: Core service for plugin discovery, loading, and lifecycle management
- **`PluginLoadContext`**: Custom assembly load context for plugin isolation

## Usage Example

Here is a minimal example of how to use the plugin engine in a host application and implement a plugin:

### ExampleHostApp/Program.cs

```csharp
using Navz.PluginEngine.Host;

class Program
{
    static void Main()
    {
        var manager = new PluginManager();
        manager.LoadPlugins("../ExamplePlugin/bin/Debug/net8.0/");
        foreach (var plugin in manager.Plugins)
        {
            plugin.Initialize(null); // Pass null or a concrete IPluginHostContext implementation
        }
    }
}
## Implementation Examples

### Basic Sync Plugin

```csharp
// Simple synchronous plugin
public class ExamplePlugin : IPlugin
{
    public string Id => "example.plugin";
    public string Name => "Example Plugin";
    public string Version => "1.0.0";

    public void Initialize(IPluginHostContext? context = null)
    {
        // Basic initialization
    }

    public void OnStop()
    {
        // Clean up resources
    }
}
```

### Async Plugin with Error Handling

```csharp
// Async plugin with proper cancellation
public class ExampleAsyncPlugin : IPluginAsync
{
    public string Id => "example.async";
    public string Name => "Async Plugin";
    public string Version => "1.0.0";

    public async Task InitializeAsync(
        IPluginHostContext? context,
        CancellationToken token)
    {
        try
        {
            // Async initialization
            await Task.CompletedTask;
        }
        catch when (token.IsCancellationRequested)
        {
            // Handle cancellation
        }
    }

    public async Task OnStopAsync(CancellationToken token)
    {
        // Async cleanup
        await Task.CompletedTask;
    }
}
```

### Host Implementation

```csharp
// Initialize plugin manager
using var manager = new PluginManager();

try
{
    // Load plugins with error handling
    await manager.LoadPluginsAsync("plugins/");

    // Initialize with optional context and cancellation
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
    await manager.InitializeAllPluginsAsync(hostContext, cts.Token);

    // Use span-based enumeration (zero allocation)
    foreach (var plugin in manager.Plugins)
    {
        Console.WriteLine($"Running: {plugin.Name}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Plugin error: {ex.Message}");
}
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code

### Installation

```bash
dotnet add package Navz.PluginEngine.Abstractions
dotnet add package Navz.PluginEngine.Host
```

### Basic Setup

1. Create plugin interfaces:

    ```csharp
    // Plugin.cs
    public interface IMyPlugin : IPlugin
    {
        void DoWork();
    }
    ```

2. Implement a plugin:

    ```csharp
    // MyPlugin.cs
    public class MyPlugin : IMyPlugin
    {
        public string Id => "my.plugin";
        public string Name => "My Plugin";
        public string Version => "1.0.0";

        public void Initialize(IPluginHostContext? context = null)
        {
            // Basic initialization
        }

        public void OnStop()
        {
            // Cleanup
        }

        public void DoWork()
        {
            // Plugin functionality
        }
    }
    ```

3. Load and use plugins:

    ```csharp
    using var manager = new PluginManager();

    // Load plugins with error handling
    try
    {
        await manager.LoadPluginsAsync("plugins/");
        await manager.InitializeAllPluginsAsync();

        // Use plugins (zero-allocation enumeration)
        foreach (var plugin in manager.Plugins)
        {
            if (plugin is IMyPlugin myPlugin)
            {
                myPlugin.DoWork();
            }
        }
    }
    finally
    {
        // Cleanup
        await manager.StopAllPluginsAsync();
    }
    ```

## Sample Projects

The solution includes example implementations:

- `ExamplePlugin`: Basic sync plugin
- `ExampleAsyncPlugin`: Plugin with async operations
- `ExampleHostApp`: Plugin loading and lifecycle

## Testing

Run the test suite:

```bash
dotnet test
```

Test projects:

- `Host.UnitTests`: Core functionality tests
- `Host.IntegrationTests`: Plugin loading tests

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to your branch
5. Create a Pull Request

## License

MIT License - See LICENSE file for details.

## Common Issues

### Plugin Loading

- Verify plugin DLLs target compatible .NET version
- Check plugin assemblies exist in the specified directory
- Ensure all plugin dependencies are available

### Memory Usage

- Use Span-based enumeration to avoid allocations
- Properly dispose PluginManager instances
- Call StopAllPluginsAsync to unload plugins

### Thread Safety

- Always use LoadPluginsAsync with async/await
- Handle plugin failures gracefully
- Use cancellation tokens for long operations

## Troubleshooting

- **Plugins not loading:**
  Ensure plugin DLLs are built for the correct .NET version and placed in the expected directory. Check for missing dependencies.

- **Assembly conflicts:**
  Use `PluginLoadContext` for isolation. If you see type conflicts, verify that plugins do not reference different versions of shared libraries.

- **Initialization errors:**
  If `Initialize` throws, check that your `IPluginHostContext` provides all required services and that plugin constructors do not have side effects.

- **Test failures:**
  Run `dotnet test` and review the output in `TestResults/`. Ensure all dependencies are restored and the test project targets the correct framework.

- **NuGet packaging issues:**
  Make sure your `.csproj` files include `<GeneratePackageOnBuild>true</GeneratePackageOnBuild>`. Check the output for `.nupkg` files in `bin/Release/`.
