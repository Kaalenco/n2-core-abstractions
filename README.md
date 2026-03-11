# n2-core

[![CodeQL](https://github.com/Kaalenco/n2-core-abstractions/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/Kaalenco/n2-core-abstractions/actions/workflows/github-code-scanning/codeql)
[![.NET Build and test](https://github.com/Kaalenco/n2-core-abstractions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Kaalenco/n2-core-abstractions/actions/workflows/dotnet.yml)

`N2.Core.Abstractions` is a shared library of interfaces, abstractions, and lightweight models for .NET projects. It was created to eliminate the pattern of each project re-implementing the same foundational contracts — response envelopes, database context wrappers, user identity, command dispatching — in slightly different ways. By defining these once in a single package, projects can depend on a common vocabulary instead of duplicating it.

The package contains no concrete service implementations. Everything is an interface, an abstract base, or a plain data model. Implementations live in separate, downstream packages.

The library targets `netstandard2.0`, `netstandard2.1`, `net8.0`, and `net10.0`.

## Functional areas

### Core utilities (`N2.Core`)

The root namespace covers the cross-cutting concerns that almost every application needs. This includes standardised result and validation envelopes that carry a value, a success flag, and a human-readable message — giving callers a consistent way to handle outcomes without relying on exceptions for control flow. Supporting types handle pagination metadata, selectable list items, and HTML-safe string wrapping. Service interfaces cover common infrastructure concerns such as email delivery, application settings, background work, logging, activity tracking, and translation of text.

### Command handling (`N2.Core.Commands`)

A lightweight command/response pattern for decoupling request producers from request processors. A conductor acts as a central dispatcher: it receives typed command requests, locates registered handlers, invokes them, and routes typed responses back through a callback mechanism. The pattern supports both synchronous invocation and async request/await-response workflows. Typed response wrappers for common return shapes (lists, pages, scalars, acknowledgements) are included so handlers do not need to invent their own envelopes.

### Database and entity abstractions (`N2.Core.Entity`)

Interfaces that describe how application code should interact with a database context without binding to a specific ORM. This covers context creation and factory patterns, unit-of-work scoping, standard model properties (identity, audit timestamps, soft-delete markers), and connection-string resolution that is environment- and tenant-aware. Code that depends only on these interfaces can be tested with in-memory fakes and swapped between ORM implementations without changes.

### Identity and access control (`N2.Core.Identity`)

Abstractions for the full authentication and authorisation surface: authenticating a user and issuing tokens, managing users and roles, resolving the current user's context within a request, evaluating access rights, and reading claims. Predefined role constants and a role/right relationship model give downstream code a consistent starting point for access policy decisions. Models for login providers, user profiles, and user-facing alerts are also included.

### HTTP abstractions (`N2.Core.Http`)

Thin wrappers around the HTTP request/response cycle, designed to make code that reads request data or writes response data testable without a real HTTP server. An additional interface covers typed API endpoint definitions so that client and server can share a contract without coupling to a specific HTTP client library.

### Document management (`N2.Core.Dms`)

Interfaces for a document management service layer. They define how documents are stored, retrieved, and described — covering both the service operations (upload, download, delete) and the repository that backs them. Attachment type classification is included. Concrete storage backends (blob storage, file system, SharePoint, etc.) implement these interfaces without the calling code needing to know which backend is active.

### Pub/sub change notification (`N2.Core.PubSub`)

A simple publish/subscribe mechanism for broadcasting change events within a process. Components that modify data publish change notifications; interested parties register as listeners without any direct coupling to the publisher. This is intended for lightweight in-process eventing, not distributed messaging.

### Exception types (`N2.Core`)

A hierarchy of typed exceptions for common error scenarios: configuration problems, contract violations (pre/post-condition failures), missing elements, general operation failures, and unauthorised access. Using these rather than generic `Exception` or `InvalidOperationException` gives catch sites enough information to respond appropriately and produce consistent error responses.

### System abstractions (`N2.Core.SystemAbstractions`)

An interface over system-level services (currently time) so that code depending on the current clock can be tested deterministically.

## Change Log

- **2024-03-29** Version 1.0.1 : Collected several abstractions and utilities into a single package.
	- Initial release of N2.Core with basic abstractions and utilities.
- **2025-04-15** Version 1.2.1 :  Isolating abstractions into separate namespaces for better organization.
	- Introduced `N2.Core.Abstractions` namespace for HTTP-related abstractions.
	- Introduced `N2.Core.Entity` namespace for database context and model abstractions.
	- Introduced `N2.Core.Identity` namespace for user authentication and role management abstractions.
	- Introduced `N2.Core.Models` namespace for response models and utilities.
	- Introduced `N2.Core.Dms` namespace for document management services abstractions.
	- Added initial documentation for each namespace and its components.
- **2025-05-28** Version 1.3.1 : Breaking changes due to namespace restructuring.
	- Added a unit test project with some initial tests.
	- Rename `N2.Core.Models` to `N2.Core.Commands` and extended command handler abstractions and base classes for command handling.
	- Extended Entity namespace with additional interfaces for database context management.
	- Extended Identity namespace with additional user and role management interfaces.
	- Add exception definitions for common error scenarios.
	- Rename 'ITextService' to 'ITranslator' to better reflect its purpose.
- **2025-06-03** Version 1.3.4 : Updates and fixes, documentation
- **2025-06-04** Version 1.3.5 : Added support for NetStandard 2.0
- **2026-03-11** Version 1.5.0 : Added support for Net10
	- Added support for .NET 10.0 and dropped support for .NET 9.0.
	- Updated documentation to reflect new version support and any breaking changes.
