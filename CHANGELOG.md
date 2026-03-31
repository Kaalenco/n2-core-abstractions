# Changelog

## 1.6.0 — 2026-03-31

Breaking changes in `IHttpClient` — all method signatures now require a `CancellationToken` parameter.

- Added `CancellationToken token` parameter to all `IHttpClient` methods: `DeleteAsync`, `GetRelativeAsync`, `ReadJsonDocumentAsync`, `GetFromUriAsync`, `PostRelativeAsync`, `PostResourceAsync`, `PutRelativeAsync`, and `PutResourceAsync`.
- Added primitive `PostAsync(Uri, HttpContent, CancellationToken)` method returning raw `HttpResponseMessage` for callers that need direct control over the response.
- Added primitive `GetAsync<TResource>(Uri, CancellationToken)` method returning raw `HttpResponseMessage`.
- Enabled NuGet audit (`NuGetAudit`, `NuGetAuditMode=All`, `NuGetAuditLevel=Low`) in the project file.
- Removed suppressions for `CA5349` (weak cryptographic algorithms) and `CA5394` (insecure randomness) — these warnings are now enforced.
- Updated `WARNINGS.md` to remove the corresponding suppression entries.

## 1.5.1 — 2026-03-31

- Added `src/WARNINGS.md` documenting all suppressed analyser warnings with severity ratings and guidance on when each suppression is safe to keep.

## 1.5.0 — 2026-03-11

- Added support for .NET 10.0 and dropped support for .NET 9.0.
- Updated documentation to reflect new version support and any breaking changes.

## 1.3.5 — 2025-06-04

- Added support for NetStandard 2.0.

## 1.3.4 — 2025-06-03

- Updates and fixes, documentation.

## 1.3.1 — 2025-05-28

Breaking changes due to namespace restructuring.

- Added a unit test project with initial tests.
- Renamed `N2.Core.Models` to `N2.Core.Commands`; extended command handler abstractions and base classes.
- Extended `N2.Core.Entity` with additional interfaces for database context management.
- Extended `N2.Core.Identity` with additional user and role management interfaces.
- Added exception definitions for common error scenarios.
- Renamed `ITextService` to `ITranslator` to better reflect its purpose.

## 1.2.1 — 2025-04-15

Isolating abstractions into separate namespaces for better organisation.

- Introduced `N2.Core.Abstractions` namespace for HTTP-related abstractions.
- Introduced `N2.Core.Entity` namespace for database context and model abstractions.
- Introduced `N2.Core.Identity` namespace for user authentication and role management abstractions.
- Introduced `N2.Core.Models` namespace for response models and utilities.
- Introduced `N2.Core.Dms` namespace for document management service abstractions.
- Added initial documentation for each namespace and its components.

## 1.0.1 — 2024-03-29

Initial release of N2.Core with basic abstractions and utilities.
