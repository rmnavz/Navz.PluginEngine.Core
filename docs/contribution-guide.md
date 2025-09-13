# Contribution Guide

## Welcome Contributors

Thank you for your interest in contributing to the Navz Plugin Engine Core! This guide will help you get started with contributing to the project.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Testing Requirements](#testing-requirements)
- [Documentation](#documentation)
- [Pull Request Process](#pull-request-process)
- [Issue Reporting](#issue-reporting)

## Code of Conduct

We are committed to providing a welcoming and inclusive experience for everyone. We expect all contributors to:

- Be respectful and constructive in discussions
- Welcome newcomers and help them get started
- Focus on what's best for the community
- Show empathy towards other contributors

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Git for version control
- Visual Studio Code, Visual Studio, or JetBrains Rider (recommended)

### Development Environment Setup

1. **Fork & Clone**

   ```bash
   git clone https://github.com/your-username/Navz.PluginEngine.Core.git
   cd Navz.PluginEngine.Core
   ```

2. **Install .NET SDK**
   - Download and install .NET 8.0 SDK or later
   - Verify installation: `dotnet --version`

3. **Configure Development Tools**
   - Install Visual Studio Code or Visual Studio
   - Recommended VS Code extensions:
     - C# Dev Kit
     - .NET Core Test Explorer
     - XML Documentation Comments

4. **Build Solution**

   ```bash
   dotnet restore
   dotnet build
   ```

5. **Run Tests**

   ```bash
   dotnet test
   ```

### Setting Up Development Environment

1. **Fork and Clone**

   ```bash
   git clone https://github.com/your-username/Navz.PluginEngine.Core.git
   cd Navz.PluginEngine.Core
   ```

2. **Build the Solution**

   ```bash
   dotnet build
   ```

3. **Run Tests**

   ```bash
   dotnet test
   ```

4. **Verify Examples Work**

   ```bash
   cd examples/ExampleHostApp
   dotnet run
   ```

## Development Workflow

### Branch Strategy

- `main`: Stable release branch
- `develop`: Integration branch for new features
- `feature/feature-name`: Feature development branches
- `bugfix/issue-number`: Bug fix branches
- `hotfix/critical-fix`: Critical production fixes

### Making Changes

1. **Create a Feature Branch**

   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make Your Changes**
   - Follow coding standards (see below)
   - Add appropriate tests
   - Update documentation

3. **Test Your Changes**

   ```bash
   dotnet build
   dotnet test
   ```

4. **Commit Your Changes**

   ```bash
   git add .
   git commit -m "feat: add new plugin discovery mechanism"
   ```

### Commit Message Format

We follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

``` markdown
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

**Types:**

- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or modifying tests
- `chore`: Maintenance tasks

**Examples:**

``` markdown
feat(host): add support for async plugin loading
fix(abstractions): resolve null reference in plugin context
docs: update README with new examples
```

## Coding Standards

### C# Code Style

We follow the [.NET coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) with some specific guidelines:

#### File Organization

```csharp
// File header comment
using System;
using System.Collections.Generic;
using ExternalLibrary;
using Navz.PluginEngine.Abstractions; // Project references last

namespace Navz.PluginEngine.Host
{
    /// <summary>
    /// XML documentation for public types
    /// </summary>
    public class ExampleClass
    {
        // Private fields first
        private readonly string _privateField;

        // Public properties
        public string PublicProperty { get; set; }

        // Constructors
        public ExampleClass(string value)
        {
            _privateField = value ?? throw new ArgumentNullException(nameof(value));
        }

        // Public methods
        public void PublicMethod()
        {
            // Implementation
        }

        // Private methods last
        private void PrivateMethod()
        {
            // Implementation
        }
    }
}
```

#### Naming Conventions

- **Classes**: PascalCase (`PluginManager`)
- **Methods**: PascalCase (`LoadPlugins`)
- **Properties**: PascalCase (`PluginCount`)
- **Fields**: camelCase with underscore prefix (`_privateField`)
- **Parameters**: camelCase (`pluginPath`)
- **Local variables**: camelCase (`pluginInstance`)
- **Constants**: PascalCase (`MaxPluginCount`)
- **Interfaces**: PascalCase with 'I' prefix (`IPlugin`)

#### Error Handling

```csharp
// Use ArgumentNullException.ThrowIfNull for parameter validation
public void Method(string parameter)
{
    ArgumentNullException.ThrowIfNull(parameter);
    // Method implementation
}

// Document exceptions in XML comments
/// <summary>
/// Loads a plugin from the specified path.
/// </summary>
/// <param name="path">The path to the plugin assembly.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null.</exception>
/// <exception cref="FileNotFoundException">Thrown when the plugin file is not found.</exception>
public void LoadPlugin(string path)
{
    // Implementation
}
```

### XML Documentation

All public APIs must have XML documentation:

```csharp
/// <summary>
/// Provides plugin discovery, loading, and lifecycle management.
/// </summary>
/// <remarks>
/// This class is thread-safe for read operations but not for write operations.
/// Use appropriate synchronization when loading/unloading plugins concurrently.
/// </remarks>
public class PluginManager
{
    /// <summary>
    /// Gets the list of loaded plugins as a read-only collection.
    /// </summary>
    /// <value>
    /// A read-only collection containing all currently loaded plugins.
    /// </value>
    public IReadOnlyList<IPluginBase> Plugins => _plugins.AsReadOnly();
}
```

## Testing Requirements

### Unit Tests

- **Coverage**: Aim for 80%+ code coverage
- **Framework**: Use xUnit.net with FluentAssertions
- **Mocking**: Use Moq for dependency mocking
- **Naming**: Use descriptive test method names

```csharp
[Fact]
public void LoadPlugins_WithValidPath_ShouldLoadAllPluginsInDirectory()
{
    // Arrange
    var pluginManager = new PluginManager();
    var testPluginPath = GetTestPluginPath();

    // Act
    pluginManager.LoadPlugins(testPluginPath);

    // Assert
    pluginManager.Plugins.Should().HaveCount(2);
    pluginManager.Plugins.Should().Contain(p => p.Name == "TestPlugin1");
}

[Fact]
public void LoadPlugins_WithNullPath_ShouldThrowArgumentNullException()
{
    // Arrange
    var pluginManager = new PluginManager();

    // Act & Assert
    Assert.Throws<ArgumentNullException>(() => pluginManager.LoadPlugins(null));
}
```

### Integration Tests

- Test complete scenarios end-to-end
- Use temporary directories for file operations
- Clean up resources in test disposal

### Test Organization

``` txt
tests/
├── Navz.PluginEngine.Host.Tests/
│   ├── PluginManagerTests.cs
│   ├── PluginLoadContextTests.cs
│   └── DefaultPluginHostContextTests.cs
└── Navz.PluginEngine.Integration.Tests/
    ├── EndToEndTests.cs
    └── SamplePluginTests.cs
```

## Documentation

### Code Documentation

- All public APIs must have XML documentation
- Use `<summary>`, `<param>`, `<returns>`, `<exception>` tags
- Include `<example>` tags for complex APIs
- Document thread safety considerations

### Markdown Documentation

- Use proper heading hierarchy
- Include code examples
- Keep lines under 100 characters when possible
- Use tables for structured information

### README Updates

When adding new features, update:

- Feature descriptions
- Usage examples
- API references
- Breaking changes (if any)

## Pull Request Process

### Before Submitting

1. **Ensure all tests pass**

   ```bash
   dotnet test --logger "console;verbosity=detailed"
   ```

2. **Check code coverage**

   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

3. **Run static analysis**

   ```bash
   dotnet build --verbosity normal
   ```

4. **Update documentation** as needed

### Pull Request Template

```markdown
## Description
Brief description of the changes and their purpose.

## Type of Change
- [ ] Bug fix (non-breaking change that fixes an issue)
- [ ] New feature (non-breaking change that adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] Manual testing completed

## Checklist
- [ ] Code follows the style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] Tests added/updated and passing
- [ ] No breaking changes or breaking changes documented
```

### Review Process

1. **Automated Checks**: CI builds and tests must pass
2. **Code Review**: At least one maintainer review required
3. **Discussion**: Address review feedback promptly
4. **Approval**: Maintainer approval required for merge

## Issue Reporting

### Bug Reports

Use the bug report template and include:

- Clear description of the issue
- Steps to reproduce
- Expected vs actual behavior
- Environment details (.NET version, OS, etc.)
- Minimal reproduction case

### Feature Requests

Use the feature request template and include:

- Clear description of the feature
- Use case and rationale
- Proposed API (if applicable)
- Alternatives considered

### Security Issues

For security-related issues:

- **Do not** create public issues
- Email security reports to: [security@navz.dev](mailto:security@navz.dev)
- Include detailed description and reproduction steps

## Release Process

### Versioning

We follow [Semantic Versioning](https://semver.org/):

- **MAJOR**: Breaking changes
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes (backward compatible)

### Release Checklist

1. Update version numbers
2. Update CHANGELOG.md
3. Create release branch
4. Final testing
5. Create GitHub release
6. Publish NuGet packages

## Getting Help

- **Documentation**: Start with README and docs/
- **Discussions**: Use GitHub Discussions for questions
- **Issues**: Create an issue for bugs or feature requests
- **Chat**: Join our community chat (link in README)

## Recognition

Contributors are recognized in:

- CHANGELOG.md for each release
- README.md contributors section
- GitHub contributors page
- Special recognition for significant contributions

Thank you for contributing to the Navz Plugin Engine Core!
