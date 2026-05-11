# N2.Core.Abstractions — Developer Guide

This document explains how to build, test, and maintain the project locally and in CI.

## Solution structure

```
src/
├── N2.Core.Abstractions/               # Main library (multi-targeted: netstandard2.0, netstandard2.1, net8.0, net10.0)
├── N2.Core.Abstractions.UnitTests/     # Unit tests (MSTest)
├── Directory.Build.props               # Shared build settings (warnings as errors, analyzers)
└── N2.Core.Abstractions.sln
```

## Building

```bash
# Restore dependencies
dotnet restore src

# Build (Debug, all targets)
dotnet build src --no-restore

# Build (Release — also produces the .nupkg)
dotnet build src --no-restore --configuration Release
```

> `GeneratePackageOnBuild` is enabled, so a `.nupkg` is created automatically on Release builds.

## Running tests

### Unit tests

```bash
dotnet test src --no-build --verbosity normal --filter "TestCategory!=Integration"
```

The `TestCategory!=Integration` filter is a convention across all N2 libraries. Integration tests are tagged `[TestCategory("Integration")]` and may require external dependencies (a running database, service, or other infrastructure) that are not available in every environment. Excluding them by default makes the unit test run safe to run anywhere without setup.

This repository currently has no integration tests, so the filter has no practical effect here — but it is included to stay consistent with the convention.

### Single test by name

```bash
dotnet test src --filter "FullyQualifiedName=N2.Core.Abstractions.UnitTests.SomeTestClass.SomeTestMethod"
```

Test results (`.trx` files) are written to `src/TestResults/`.

## CI/CD

Pipeline: `pipelines/azure-pipelines.yml` — registered in Azure DevOps under **N2 Vault services**.

Triggers on push and PR to `trunk` when files under `src/N2.*/**` change. Commits that touch only documentation or the pipeline file itself do not trigger a run.

| # | Stage | Runs on |
|---|-------|---------|
| 1 | **Build and Test** | push + PR to `trunk` |
| 2 | **Code Analysis** | push + PR to `trunk` (parallel with Build) |
| 3 | **Publish** | push to `trunk` only, after Build passes |

The Publish stage downloads the `.nupkg` artifact from the Build stage and pushes it to the Azure Artifacts feed. Re-pushing an existing version is a no-op (`--skip-duplicate`).

### Pipeline variables

| Variable | Description | Value |
|----------|-------------|-------|
| `artifactsFeedName` | Azure Artifacts feed name | `vault-packages` |
| `buildConfiguration` | MSBuild configuration | `Release` |

## Regular maintenance

### Pipeline task versions

The pipeline pins specific Azure DevOps task major versions (e.g. `DotNetCoreCLI@2`, `Cache@2`, `UseDotNet@2`). Periodically check the Azure DevOps Marketplace for new major releases and review the changelog for breaking changes before bumping.

### .NET SDK targets

The library targets `netstandard2.0`, `netstandard2.1`, `net8.0`, and `net10.0`. When a .NET version reaches end-of-life:
1. Remove its `<TargetFramework>` entry from `N2.Core.Abstractions.csproj`.
2. Remove the matching `UseDotNet@2` step from the pipeline.

The `netstandard` targets are built by the .NET 8 and .NET 10 SDKs and do not require separate setup steps.

### NuGet package version

The package version is set in `N2.Core.Abstractions/N2.Core.Abstractions.csproj`:

```xml
<PackageVersion>1.7.1</PackageVersion>
<AssemblyVersion>1.7.1</AssemblyVersion>
<ProductVersion>1.7.1</ProductVersion>
```

Bump all three values together before merging a release to `trunk`.

### NuGet dependencies

Update package references when security advisories are published. `NuGetAudit` is enabled with `NuGetAuditLevel=Low` — advisories surface as build errors. Check the advisory, update the affected package, and verify the build passes.
