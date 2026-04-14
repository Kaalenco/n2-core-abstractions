# Changelog

## 1.7.0 — 2026-04-14

### Identity

- Extended `IWebTokenGenerator` with `GenerateRefreshToken()` (returns a cryptographically random 88-character Base64 string) and `RefreshTokenExpiry()` (returns the configured expiry `DateTime` for a new refresh token). The caller is responsible for persisting the refresh token and associating it with the user.
- Extended `IUserContext` with full multi-tenant support: `CurrentTenantId`, `CurrentTenantName`, `SetTenantContext(Guid)`, `SetTenantContext(string)`, `IsInTenant(Guid)`, `IsInTenant(string)`, and `TenantMemberships` (enumerates all tenant memberships with per-tenant role lists). Role-check methods (`IsAdmin`, `CanPublish`, etc.) evaluate against the active tenant; call `SetTenantContext` before performing role checks.

### PubSub

- Refactored `IItemChanged` — added XML documentation; properties are now `DateTime DateTime`, `Type Type`, `Guid Uuid`.
- `ItemChanged` is now a `readonly struct` implementing `IItemChanged` and `IEquatable<ItemChanged>`. Equality is based on `Type` and `Uuid` only (timestamp is excluded). Includes `==`/`!=` operators and a multi-target `GetHashCode` (`HashCode.Combine` on .NET 8+, manual prime-number hash on older targets).
- **Breaking:** `INotifyChangeListener.OnItemModified` now returns `bool` instead of `void` — `true` if the listener processed the notification, `false` if it ignored it (e.g. type mismatch).
- **Breaking:** `INotifyChangeService.ItemModified<T>` now returns `int` (number of listeners that processed the notification) instead of `void`.
- **Breaking:** `INotifyChangeService.AddSubscription` now returns `bool` (`true` if newly registered, `false` if already present) instead of `void`.
- **Breaking:** `INotifyChangeService.RemoveSubscription` now returns `bool` (`true` if found and removed, `false` if not registered) instead of `void`.

### Identity (IIdentityManager)

- **Breaking:** `LogoffUser` now returns `Task<ICommandResponse>` instead of `Task`.
- **Breaking:** `RegisterRefreshToken` now returns `Task<ICommandResponse>` instead of `Task`.
- **Breaking:** `GetUserSecret` now returns `Task<ICommandResponse<string>>` instead of `Task<string>`.

### HTTP (1.6.0)

Breaking: all `IHttpClient` method signatures now require a `CancellationToken` parameter.

- Added `CancellationToken token` to `DeleteAsync`, `GetRelativeAsync`, `ReadJsonDocumentAsync`, `GetFromUriAsync`, `PostRelativeAsync`, `PostResourceAsync`, `PutRelativeAsync`, and `PutResourceAsync`.
- Added primitive `PostAsync(Uri, HttpContent, CancellationToken)` returning raw `HttpResponseMessage`.
- Added primitive `GetAsync<TResource>(Uri, CancellationToken)` returning raw `HttpResponseMessage`.
- Enabled NuGet audit (`NuGetAudit`, `NuGetAuditMode=All`, `NuGetAuditLevel=Low`) in the project file.
- Removed suppressions for `CA5349` and `CA5394` — weak-crypto and insecure-randomness warnings are now enforced.

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
