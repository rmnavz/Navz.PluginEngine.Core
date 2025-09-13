# Architectural Overview

## Introduction

The Navz Plugin Engine Core is a lightweight, memory-optimized plugin system for .NET applications. It focuses on efficient plugin loading, assembly isolation, and basic lifecycle management. The system follows three core principles:

1. **Memory Efficiency**: Zero-allocation enumeration and array-based storage
2. **Assembly Isolation**: Each plugin runs in its own AssemblyLoadContext
3. **Thread Safety**: Thread-safe plugin operations and lifecycle management

## Core Architecture

### 1. Two-Layer Architecture

```text
┌─────────────────────────────────────────────────────────────┐
│                    HOST APPLICATION                         │
│  ┌─────────────────────────────────────────────────────────┐│
│  │              Application Layer                          ││
│  │  • Business Logic                                       ││
│  │  • Host Context (Optional)                              ││
│  │  • Plugin Discovery                                     ││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                 PLUGIN ENGINE LAYER                         │
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
│  │   IPlugin       │    │   IPluginAsync  │                │
│  └─────────────────┘    └─────────────────┘                │
└─────────────────────────────────────────────────────────────┘
```

### 2. Key Implementation Features

The engine implements these key features:

1. **Memory-Efficient Storage**:

   - Uses arrays for plugin storage
   - Zero-allocation enumeration with Span
   - Automatic array resizing
   - Thread-safe operations

2. **Assembly Isolation**:

   - Per-plugin AssemblyLoadContext
   - Clean assembly unloading
   - Memory leak prevention
   - Basic assembly resolution

3. **Minimal Plugin API**:

   - IPluginBase: Core metadata
   - IPlugin: Sync operations
   - IPluginAsync: Async support
   - Basic host context

### 3. Core Components

#### Navz.PluginEngine.Abstractions

**Role**: Core plugin contracts

**Dependencies**: None

**Key Features**:

- IPlugin/IPluginAsync interfaces
- Basic host context definition
- Shared metadata types

#### Navz.PluginEngine.Host

**Role**: Plugin management engine

**Dependencies**: Abstractions only

**Key Features**:

- Memory-optimized plugin storage
- Basic plugin loading and lifecycle
- Thread-safe operations
- Assembly isolation

## Performance Characteristics

### Memory Efficiency

1. **Array-Based Storage**:
   - Fixed-size arrays for plugin storage
   - Automatic resizing when needed
   - Minimal allocations during operation

2. **Zero-Allocation Enumeration**:
   - ReadOnlySpan for plugin access
   - No temporary array allocations
   - Efficient iteration support

### Thread Safety

1. **Synchronized Operations**:
   - Thread-safe plugin loading
   - Safe array resizing
   - Protected plugin enumeration

2. **Graceful Error Handling**:
   - Continue on plugin failure
   - Safe cleanup on errors
   - Logging of important events

### Plugin Lifecycle

1. **Loading**:
   - Direct assembly loading
   - Type scanning and validation
   - Safe context creation

2. **Initialization**:
   - Support for async/sync plugins
   - Optional host context
   - Error isolation

3. **Shutdown**:
   - Clean plugin stopping
   - Assembly context disposal
   - Resource cleanup

## Current Scope

### Implemented Features

1. **Core Plugin Management**:
   - Basic plugin loading
   - Assembly isolation
   - Lifecycle management
   - Thread safety

2. **Memory Optimization**:
   - Array-based storage
   - Zero-allocation access
   - Clean unloading

3. **Error Handling**:
   - Graceful failure handling
   - Isolated plugin errors
   - Safe cleanup

### Limitations

1. **Basic Features Only**:
   - Simple plugin discovery
   - Basic host context
   - Manual dependency management

2. **Not Yet Implemented**:
   - Advanced configuration
   - Plugin-to-plugin communication
   - Hot reload support
   - Advanced dependency resolution

3. **Security Features**:
   - Basic assembly isolation
   - Simple host protection
   - Manual resource limits
