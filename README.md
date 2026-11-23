# n2-core-abstractions

[![Build Status](https://dev.azure.com/gjkaal/N2%20Vault%20services/_apis/build/status%2FN2.Core.Abstractions%20-%20Publish?branchName=trunk)](https://dev.azure.com/gjkaal/N2%20Vault%20services/_build/latest?definitionId=2&branchName=trunk)
[![N2.Core.Abstractions package in vault-packages@Release feed in Azure Artifacts](https://feeds.dev.azure.com/gjkaal/3b3decf8-eba1-45d0-b47a-d9c1ce84f739/_apis/public/Packaging/Feeds/vault-packages@Release/Packages/5a9ef012-1b00-43cd-948b-d9483f3bfad7/Badge)](https://dev.azure.com/gjkaal/N2%20Vault%20services/_artifacts/feed/vault-packages@Release/NuGet/N2.Core.Abstractions?preferRelease=true)

`N2.Core.Abstractions` is a shared library of interfaces, abstractions, and lightweight models for .NET projects. It was created to eliminate the pattern of each project re-implementing the same foundational contracts — response envelopes, database context wrappers, user identity, command dispatching — in slightly different ways. By defining these once in a single package, projects can depend on a common vocabulary instead of duplicating it.

The package contains no concrete service implementations. Everything is an interface, an abstract base, or a plain data model. Implementations live in separate, downstream packages.

The library targets `netstandard2.0`, `netstandard2.1`, `net10.0`, and `net11.0`.


From 2.0.0 every asynchronous member takes a required `CancellationToken` as its last parameter, and no member name ends in `Async`. See [CHANGELOG.md](CHANGELOG.md) for the full list of renamed members.

## Upgrading to 2.0

2.0.0 is a breaking release: renamed members, required cancellation tokens, and `net8.0`
replaced by `net10.0`/`net11.0`. Follow [MIGRATION-2.0.md](MIGRATION-2.0.md) for the upgrade
checklist, the 1.x -> 2.0.0 member mapping with before/after examples, and guidance on passing
cancellation tokens.

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

### Encryption utilities (`N2.Core.Abstractions.Encryption`)

A small set of cryptographic utilities. `AuthenticatedValue` wraps an arbitrary byte payload with a mandatory expiration and serialises it to a compact base64url token using AES-256-CBC encryption and HMAC-SHA256 authentication (Encrypt-then-MAC). `EncryptionUtilities` provides helper methods such as generating cryptographically random keys.

### System abstractions (`N2.Core.SystemAbstractions`)

An interface over system-level services (currently time) so that code depending on the current clock can be tested deterministically.

## Project documentation

| Document | Description |
|----------|-------------|
| [src/DEVELOPER-GUIDE.md](src/DEVELOPER-GUIDE.md) | Developer guide — build commands, CI/CD pipeline, and maintenance notes. |
| [src/WARNINGS.md](src/WARNINGS.md) | Suppressed analyser warnings — lists every entry in `NoWarn` with severity rating and guidance on when each suppression is safe to keep. |
| [CHANGELOG.md](CHANGELOG.md) | Full version history. |

## Change log

See [CHANGELOG.md](CHANGELOG.md) for the full history. Current version: **3.0.0**.
