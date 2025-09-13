# Navz Plugin Engine Core Documentation

Welcome to the comprehensive documentation for the Navz Plugin Engine Core! This directory contains detailed guides, architectural overviews, and best practices for using and contributing to the plugin engine.

## Documentation Sections

### Architecture

- [Architectural Overview](architecture/overview.md) - High-level architecture and design decisions

### API Reference

The API reference section contains detailed documentation for all public APIs:

- [Navz.PluginEngine.Abstractions](../src/Navz.PluginEngine.Abstractions/README.Abstractions.md)
- [Navz.PluginEngine.Host](../src/Navz.PluginEngine.Host/README.Host.md)

### Developer Guides

- [Example Applications](../examples/) - Working examples and reference implementations
- `ExamplePlugin/` - Synchronous plugin implementation
- `ExampleAsyncPlugin/` - Asynchronous plugin implementation
- `ExampleHostApp/` - Complete host application example

### Contributing

- [Contribution Guide](contributing/guide.md) - Guidelines for contributing to the project
- [Workflow Plan](workflow-plan.md) - Development workflow and branching strategy
- [CI/CD Pipeline](../.github/workflows/README.md) - Build and deployment automation

## Key Features

### 🔒 Plugin Isolation

Each plugin runs in its own `AssemblyLoadContext` for:

- Dependency conflict prevention
- Clean unloading and memory management
- Version independence between plugins
- Security boundaries

### 🔄 Asynchronous Support

- Support for both synchronous and asynchronous plugins
- Efficient resource management
- Non-blocking operations

### 🛠️ Simple Integration

- Minimal core interfaces
- Clean architecture principles
- Strong separation of concerns
- Dependency injection ready

## Quick Links

- [Main Project README](../README.md)
- [Change Log](../CHANGELOG.md)
- [License](../LICENSE)
- **`feature/*`**: New feature development
- **`bugfix/*`**: Bug fixes
- **`release/*`**: Release preparation
- **`hotfix/*`**: Critical production fixes

### Release Process

1. **Feature Development**: Work on `feature/*` branches
2. **Integration**: Merge to `develop` for testing
3. **Release Preparation**: Create `release/*` branch for final testing
4. **Production Release**: Merge to `main` and tag
5. **Automated Deployment**: CI/CD handles package publishing and release creation

## 🤝 Community & Support

### Getting Help

- **📖 Documentation**: Start here for comprehensive guides
- **💬 GitHub Discussions**: Ask questions and share ideas
- **🐛 Issues**: Report bugs or request features
- **📧 Maintainers**: Contact the core team for sensitive issues

### How to Contribute

We welcome contributions of all kinds:

- **🐛 Bug Reports**: Help us improve quality
- **✨ Feature Requests**: Suggest enhancements
- **📝 Documentation**: Improve guides and examples
- **🔧 Code Contributions**: Submit pull requests
- **🧪 Testing**: Help with testing and validation

See the [Contribution Guide](contribution-guide.md) for detailed information.

## 📄 API Documentation

### Core Interfaces

- **`IPluginBase`**: Common plugin metadata and identification
- **`IPlugin`**: Synchronous plugin lifecycle management
- **`IPluginAsync`**: Asynchronous plugin lifecycle management
- **`IPluginHostContext`**: Host-plugin communication channel

### Key Classes

- **`PluginManager`**: Central plugin discovery and lifecycle management
- **`PluginLoadContext`**: Assembly isolation and dependency management
- **`DefaultPluginHostContext`**: Default host context implementation

For complete API documentation, refer to the XML documentation in the source code and generated documentation.

## 🎯 Best Practices

### Plugin Development

- **Keep dependencies minimal** to avoid conflicts
- **Implement proper cleanup** in `OnStop`/`OnStopAsync`
- **Use the host context** for service resolution
- **Follow async patterns** when using `IPluginAsync`

### Host Integration

- **Configure isolation properly** for security and stability
- **Provide meaningful services** through the host context
- **Handle plugin failures gracefully**
- **Monitor plugin performance** and resource usage

### Development

- **Follow the coding standards** outlined in the contribution guide
- **Write comprehensive tests** for new features
- **Update documentation** for API changes
- **Use the CI/CD feedback** to maintain quality

---

📖 **Happy coding!** If you have questions or suggestions for improving this documentation, please open an issue or start a discussion.
