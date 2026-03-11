# 00001. Centralize shared abstractions and lightweight interfaces in a dedicated base library

2026-03-11

## Status

__Accepted__

## Context

Across multiple .NET projects in the Kaalenco ecosystem, the same foundational contracts kept being re-implemented independently: result envelopes, validation wrappers, database context interfaces, user identity contracts, command dispatching patterns, and common service interfaces such as email, logging, and settings. Each project had its own slightly different version, making it impossible to share code between them and creating ongoing maintenance overhead when the same concept needed to change in multiple places.

The recurring duplication indicated that these abstractions were not project-specific concerns but general-purpose building blocks that belong at a lower layer of the architecture.

## Decision

We will maintain a dedicated NuGet package — `N2.Core.Abstractions` — that owns all shared interfaces, abstract base types, and lightweight model classes used across projects. The package will:

- contain **only** interfaces, abstract bases, and plain data models; no concrete service implementations
- be multi-targeted so it can be consumed by both modern (`net8.0`, `net10.0`) and legacy (`netstandard2.0`, `netstandard2.1`) projects
- be organized into focused namespaces by functional area (commands, entity, identity, HTTP, document management, pub/sub) so consumers can understand the scope of each group at a glance
- treat all compiler warnings as errors and run analysis in `All` mode to keep the contract surface clean and consistent

Concrete implementations of these interfaces live in separate, downstream packages that depend on this one.

## Consequences

- Any project that depends on `N2.Core.Abstractions` can interoperate with any other project on the same package version without needing to duplicate or translate types.
- Adding a new cross-cutting abstraction has a single, obvious home rather than requiring a decision about where it belongs each time.
- Breaking changes to interfaces require a version bump and coordinated upgrades across all dependent projects, so the interfaces must be designed carefully and changed conservatively.
- Concrete implementations remain fully decoupled from each other; only this package forms a shared dependency.
