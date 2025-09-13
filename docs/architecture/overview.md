# Architectural Overview

## Introduction

The Navz Plugin Engine Core is designed as a minimal, efficient, and extensible plugin system for .NET applications. It follows a clean architecture approach with strong separation of concerns and dependency inversion principles. The system is built around three fundamental principles:

1. **Isolation**: Plugins operate in isolated assembly contexts to prevent dependency conflicts and enable safe unloading
2. **Flexibility**: Support for both synchronous and asynchronous plugins to accommodate different use cases
3. **Simplicity**: Minimal core interfaces to ensure long-term maintainability and backward compatibility

## Core Architecture

### 1. Three-Layer Architecture

```text
┌─────────────────────────────────────────────────────────────┐
│                    HOST APPLICATION                         │
│  ┌─────────────────────────────────────────────────────────┐│
│  │              Application Layer                          ││
│  │  • Business Logic                                       ││
│  │  • Host Services                                        ││
│  │  • Configuration                                        ││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                 PLUGIN ENGINE LAYER                         │
│  ┌─────────────────┐    ┌─────────────────────────────────┐ │
│  │ PluginManager   │    │   DefaultPluginHostContext     │ │
│  │ • Load Plugins  │    │   • Service Resolution         │ │
│  │ • Initialize    │    │   • Host Information           │ │
│  │ • Stop/Cleanup  │    │   • Plugin Communication       │ │
│  └─────────────────┘    └─────────────────────────────────┘ │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │              PluginLoadContext                          │ │
│  │              • Assembly Isolation                      │ │
│  │              • Dependency Management                   │ │
│  │              • Memory Management                       │ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                  ABSTRACTIONS LAYER                        │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │                   Core Interfaces                      │ │
│  │  • IPlugin / IPluginAsync                              │ │
│  │  • IPluginBase                                         │ │
│  │  • IPluginHostContext                                  │ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                      PLUGINS                               │
│  ┌─────────────────┐    ┌─────────────────┐                │
│  │   Plugin A      │    │   Plugin B      │    ...         │
│  │   (Sync)        │    │   (Async)       │                │
│  └─────────────────┘    └─────────────────┘                │
└─────────────────────────────────────────────────────────────┘
```

### 2. Assembly Isolation Strategy

Each plugin is loaded in its own `AssemblyLoadContext` to provide:

- **Isolation**: Prevents dependency conflicts between plugins
- **Unloading**: Enables clean plugin unloading and memory reclamation
- **Versioning**: Allows different plugins to use different versions of dependencies
- **Security**: Provides boundaries for plugin execution

```text
┌─────────────────────────────────────────────────────────────┐
│                    Default Context                          │
│  • Host Application                                         │
│  • Navz.PluginEngine.Abstractions (Shared)                 │
│  • Navz.PluginEngine.Host                                  │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Plugin A       │    │  Plugin B       │    │  Plugin C       │
│  LoadContext    │    │  LoadContext    │    │  LoadContext    │
│  • Plugin A.dll │    │  • Plugin B.dll │    │  • Plugin C.dll │
│  • Dependencies │    │  • Dependencies │    │  • Dependencies │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## Key Design Decisions

### 1. Interface-First Design

The system is built around stable interfaces that form contracts between components:

- **IPluginBase**: Common metadata for all plugins
- **IPlugin**: Synchronous plugin operations
- **IPluginAsync**: Asynchronous plugin operations
- **IPluginHostContext**: Host service access for plugins
