# Development Workflow

## Branch Structure

- **main**: Production-ready code with latest stable release
- **develop**: Integration branch for active development
- **feature/***: New features and improvements
- **fix/***: Bug fixes and patches

## Basic CI Pipeline

1. **Build**:
   - Restore dependencies
   - Build solution
   - Run tests
   - Pack NuGet packages

2. **Verify**:
   - Unit tests
   - Integration tests
   - Code formatting
   - XML documentation

3. **Release** (on tags):
   - Create NuGet packages
   - Push to NuGet.org
   - Create GitHub release
   - Update documentation

## Version Management

We use [Semantic Versioning](https://semver.org/):

- **MAJOR**: Breaking changes
- **MINOR**: New features
- **PATCH**: Bug fixes

Version format: `vX.Y.Z` (e.g., `v1.0.0`)

## Release Process

1. Update CHANGELOG.md
2. Tag release in Git
3. Push tag to trigger CI
4. Verify package on NuGet.org

- **Patch:**
  - Use `hotfix/*` branches for urgent fixes, merging into `main` and `develop`.
  - Tag as `vX.Y.Z`.
- **Develop Release:**
  - Automate builds and releases from `develop` only when changes are made (on push or merge).
  - Tag as `vX.Y.Z-develop` or use a build number/date for traceability.
  - Publish to a development or preview feed for early testing.

These practices support early testing, urgent fixes, and continuous delivery. Automate with GitHub Actions by triggering workflows on schedule, tags, or branch patterns.

---

---

This document should be updated as workflows evolve. Consider adding more details or diagrams as needed.
