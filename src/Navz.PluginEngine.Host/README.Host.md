# Navz.PluginEngine.Host

## Overview

The Navz.PluginEngine.Host package provides a lightweight, memory-optimized implementation for loading and managing plugins in .NET applications. It focuses on assembly isolation and basic plugin lifecycle management.

## Core Components

### PluginManager

Lightweight plugin management service optimized for minimal memory footprint:

- Zero-allocation plugin enumeration using `Span<T>`
- Efficient array-based storage with automatic resizing
- Basic lifecycle operations (Load/Initialize/Stop)
- Clean unloading support
- Thread-safe plugin operations

### PluginLoadContext

Custom AssemblyLoadContext implementation for plugin isolation:

- Per-plugin assembly isolation
- Efficient assembly caching with weak references
- Clean assembly unloading support
- Memory leak prevention

### DefaultPluginHostContext

Basic IPluginHostContext implementation:

- Host information provision
- Simple service resolution
- Minimal host-plugin communication

## Features

### Performance Optimizations

- Zero-allocation plugin enumeration
- Array-based storage for better memory locality
- Aggressive method inlining
- Efficient assembly caching
- Thread-safe operations with minimal locking

### Assembly Management

- Per-plugin assembly isolation
- Clean assembly unloading
- Memory-efficient resource management
- Proper dependency resolution

### Plugin Support

- Both synchronous and asynchronous plugins
- Basic lifecycle management
- Clean plugin unloading
- Exception handling with graceful degradation

## Technical Details

### Memory Management

- Uses arrays instead of lists for better performance
- Implements efficient resizing strategies
- Proper cleanup on unload
- Weak reference caching for assemblies

### Thread Safety

- Thread-safe plugin operations
- Minimal locking for better performance
- Safe concurrent plugin access
- Protected resource cleanup

### Requirements

- .NET 8.0 or above
- MIT licensed

## Getting Started

Reference this package in your .NET project to implement a plugin host. See the [project repository](https://github.com/rmnavz/Navz.PluginEngine.Core) for usage examples and documentation.

## License

This project is licensed under the MIT License.
