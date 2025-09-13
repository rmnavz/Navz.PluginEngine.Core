# Navz.PluginEngine.Abstractions

## Overview

The Navz.PluginEngine.Abstractions package provides minimal, efficient interfaces for plugin development in .NET applications. It focuses on essential contracts to support memory-optimized plugin management and basic lifecycle operations.

## Core Interfaces

### IPluginBase

Core metadata interface:

- `Id`: Unique plugin identifier
- `Name`: Display name
- `Version`: Version string

### IPlugin

Synchronous plugin contract:

- `Initialize(IPluginHostContext?)`: Basic setup
- `OnStop()`: Resource cleanup

### IPluginAsync

Asynchronous plugin support:

- `InitializeAsync(IPluginHostContext?, CancellationToken)`: Async initialization
- `OnStopAsync(CancellationToken)`: Async cleanup with cancellation

### IPluginHostContext

Simple host communication:

- `GetService<T>()`: Basic service resolution
- `GetService(Type)`: Alternative resolution method

## Features

- Minimal interface design
- Memory-efficient contracts
- Support for both sync/async plugins
- Basic host communication
- .NET 8.0+ targeting

## Getting Started

Reference this package in your .NET project to implement plugins compatible with the Navz Plugin Engine. See the [project repository](https://github.com/rmnavz/Navz.PluginEngine.Core) for usage examples and documentation.

## License

This project is licensed under the MIT License.
