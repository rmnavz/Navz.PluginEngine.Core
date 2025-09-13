# Plugin Isolation Guide

This guide explains how the Navz Plugin Engine implements plugin isolation for safe and efficient plugin management.

## Overview

Plugin isolation in Navz Plugin Engine:

- Provides assembly-level isolation between plugins
- Enables clean plugin unloading
- Prevents assembly conflicts
- Manages plugin lifecycles efficiently

## Implementation Details

The plugin engine uses a custom `PluginLoadContext` derived from `AssemblyLoadContext` to provide assembly isolation:

```csharp
// Basic plugin loading with isolation
var loadContext = new PluginLoadContext(assemblyPath);
var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);
```

### Core Features

1. Per-Plugin Isolation
   - Each plugin gets its own AssemblyLoadContext
   - Plugins can't interfere with each other's assemblies
   - Clean unloading when plugins are stopped

2. Memory Management
   - Efficient array-based plugin storage
   - Proper cleanup during unload
   - Automatic context disposal

3. Lifecycle Management
   - Safe initialization and shutdown
   - Support for both sync and async plugins
   - Graceful error handling

## Basic Usage

```csharp
// Create a plugin manager
var manager = new PluginManager();

// Load plugins - each gets its own context
await manager.LoadPluginsAsync("plugins/");

// Initialize plugins with optional host context
await manager.InitializeAllPluginsAsync(hostContext);

// Later: Stop and unload plugins
await manager.StopAllPluginsAsync();
```

## Best Practices

### Dependency Management

1. Keep Dependencies Minimal
   - Include only necessary dependencies
   - Use assembly linking when possible
   - Consider using shared assemblies for common dependencies

2. Version Management
   - Use specific versions in plugin projects
   - Document version requirements
   - Test with different dependency versions

### Resource Management

1. Memory
   - Implement proper IDisposable patterns
   - Clean up resources in OnStop
   - Monitor memory usage
   - Handle large objects appropriately

2. Threading
   - Use proper synchronization
   - Clean up threads on shutdown
   - Implement cancellation
   - Respect thread limits

## Troubleshooting

### Common Issues

1. Assembly Loading Issues
   - Check assembly versions
   - Verify assembly locations
   - Check for missing dependencies
   - Review assembly binding logs

2. Resource Leaks
   - Monitor memory usage
   - Check for unmanaged resources
   - Verify dispose patterns
   - Use memory profiling tools

3. Performance Issues
   - Monitor CPU usage
   - Check thread usage
   - Review I/O operations
   - Profile plugin startup/shutdown

## Security Considerations

### Recommendations

1. Assembly Validation
   - Implement strong naming
   - Verify assembly signatures
   - Check assembly sources
   - Implement allowlists

2. Resource Access
   - Limit file system access
   - Control network access
   - Manage registry access
   - Monitor resource usage

3. Host Protection
   - Implement timeouts
   - Handle plugin crashes
   - Monitor plugin behavior
   - Implement resource limits
