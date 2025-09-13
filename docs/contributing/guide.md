# Contribution Guide

## Welcome Contributors!

Thank you for your interest in contributing to the Navz Plugin Engine Core! This guide will help you get started with contributing to the project.

## Table of Contents

- [Contribution Guide](#contribution-guide)
  - [Welcome Contributors!](#welcome-contributors)
  - [Table of Contents](#table-of-contents)
  - [Code of Conduct](#code-of-conduct)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Development Environment Setup](#development-environment-setup)
    - [Setting Up Development Environment](#setting-up-development-environment)
  - [Development Workflow](#development-workflow)
    - [Branch Strategy](#branch-strategy)

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
     - GitHub Actions

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
