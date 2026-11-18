# Changelog

## 2.0.0 - 2026-11-17

Major release. Every asynchronous member takes a required `CancellationToken`, the `Async`
suffix is gone from all member names, and the target frameworks move to .NET 10 / .NET 11.
All changes below are breaking unless stated otherwise.

### Convention

- Every member returning `Task`, `ValueTask` or `IAsyncEnumerable` takes a `CancellationToken`
  as its last parameter. The parameter is required: no default value and no overload without it.
- No member name ends in `Async`.

### Targets and packaging

- **Breaking:** target frameworks are now `netstandard2.0`, `netstandard2.1`, `net10.0` and
  `net11.0`. `net8.0` is dropped; .NET 8 and .NET 9 consumers resolve the `netstandard2.1` asset
  and get the 10.0.x `System.Text.Json`, `System.Text.Encodings.Web` and
  `Microsoft.Extensions.Primitives` packages transitively.
- `System.Text.Json` and `System.Text.Encodings.Web` are referenced only for the `netstandard`
  targets (in-box on net10.0+). `Microsoft.Extensions.Primitives` (`StringValues`) is referenced
  for the `netstandard` targets and `net10.0`; it is in-box in `Microsoft.NETCore.App` from
  net11.0, so `net11.0` has no package reference (avoids NU1510 under package pruning).
- Package updates: `System.IO.Abstractions` 22.1.0 -> 23.0.1; `System.Text.Json`,
  `System.Text.Encodings.Web`, `Microsoft.Extensions.Primitives` 10.0.5 -> 10.0.15. These stay
  on the 10.0.x band so net10 consumers (ASP.NET Core 10 shared framework) are not lifted to
  11.0.0 assemblies.
- Publishing stays on the `vault-packages` Azure Artifacts feed. A nuget.org release is deferred:
  nuget.org recommends Trusted Publishing (GitHub Actions only) over API keys.
- Pipeline: .NET SDK 11.0.x replaces 8.0.x (10.0.x kept); coverage runs on net10.0.

### Commands (`N2.Core.Commands`)

- `ICommandHandler<TQ, TA>.WaitForAsync(TQ)` -> `WaitFor(TQ, CancellationToken)`

### Entity (`N2.Core.Entity`)

- `IUnitOfWork.Complete()` -> `Complete(CancellationToken)`
- `ICoreDataContext.DeleteAsync<T>(Guid)` -> `Delete<T>(Guid, CancellationToken)`
- `ICoreDataContext.FindRecordAsync<T>(Guid)` -> `FindRecord<T>(Guid, CancellationToken)`
- `ICoreDataContext.GetSelectListAsync(string)` -> `GetSelectList(string, CancellationToken)`
- `ICoreDataContextFactory<TContext>.CreateAsync(string)` -> `Create(string, CancellationToken)`
- `ICoreDataContextFactory<TContext>.CreateAsync()` -> `Create(CancellationToken)`
- `IDataRepository<T>.CreateAndReturn(T)` -> `CreateAndReturn(T, CancellationToken)`
- `IDataRepository<T>.UpdateAndReturn(int, T)` -> `UpdateAndReturn(int, T, CancellationToken)`
- `IDesignComponent<TList, TDetails>.SaveItemAsync(TDetails, IUserContext?)` -> `SaveItem(TDetails, IUserContext?, CancellationToken)`
- `IDesignComponent<TList, TDetails>.RemoveItemAsync(Guid, IUserContext?)` -> `RemoveItem(Guid, IUserContext?, CancellationToken)`
- `IDesignComponent<TList, TDetails>.InitializeModelAsync(string?, IUserContext?)` -> `InitializeModel(string?, IUserContext?, CancellationToken)`
- `IDesignComponent<TList, TDetails>.LoadItemsAsync(PagingInfo, IUserContext?)` -> `LoadItems(PagingInfo, IUserContext?, CancellationToken)`
- `IDesignComponent<TList, TDetails>.ReadFromDatabaseAsync(Guid, IUserContext?)` -> `ReadFromDatabase(Guid, IUserContext?, CancellationToken)`

### Document management (`N2.Core.Dms`)

- `IDocumentRepository.CompleteAsync(IUserContext, CancellationToken)` -> `Complete(IUserContext, CancellationToken)`
- `IDocumentRepository.FindDocumentAsync(...)` (4 overloads) -> `FindDocument(...)`, parameters unchanged
- `IDocumentService.FindDocumentsAsync(string, IEnumerable<string>, string, bool)` -> `FindDocuments(string, IEnumerable<string>, string, bool, CancellationToken)`
- `IDocumentService.DeleteDocumentAsync(Guid)` -> `DeleteDocument(Guid, CancellationToken)`
- `IDocumentService.SaveDocumentAsync(Stream, DocumentInformation)` -> `SaveDocument(Stream, DocumentInformation, CancellationToken)`
- `IDocumentService.UpdateDocumentAsync(Guid, DocumentInformation)` -> `UpdateDocument(Guid, DocumentInformation, CancellationToken)`
- `IDocumentService.GetDocumentInformationAsync(Guid)` -> `GetDocumentInformation(Guid, CancellationToken)`

### HTTP (`N2.Core.Http`)

- `IHttpClient.DeleteAsync` -> `Delete`
- `IHttpClient.GetRelativeAsync<TResponse>` -> `GetRelative<TResponse>`
- `IHttpClient.ReadJsonDocumentAsync` -> `ReadJsonDocument`
- `IHttpClient.GetFromUriAsync<TResource>` -> `GetFromUri<TResource>`
- `IHttpClient.PostRelativeAsync<TRequest, TResponse>` -> `PostRelative<TRequest, TResponse>`
- `IHttpClient.PostResourceAsync<TResource>` -> `PostResource<TResource>`
- `IHttpClient.PostAsync(Uri, HttpContent, CancellationToken)` -> `Post(Uri, HttpContent, CancellationToken)`
- `IHttpClient.GetAsync(Uri, CancellationToken)` -> `GetResponseMessage(Uri, CancellationToken)`
  (not `Get`: that is a reserved keyword in Visual Basic, analyzer CA1716)
- `IHttpClient.PutRelativeAsync<TRequest, TResponse>` -> `PutRelative<TRequest, TResponse>`
- `IHttpClient.PutResourceAsync<TResource>` -> `PutResource<TResource>`
- `IHttpRequest.ReadAsStringAsync(CancellationToken = default)` -> `ReadAsString(CancellationToken)`; the token is now required
- `IHttpResponse.WriteAsync(string, CancellationToken = default)` -> `Write(string, CancellationToken)`; the token is now required
- `IHttpResponse.WriteAsync(byte[], CancellationToken = default)` -> `Write(byte[], CancellationToken)`; the token is now required
- `IHttpResponse.WriteAsJsonAsync<T>(T, JsonSerializerOptions, CancellationToken = default)` -> `WriteAsJson<T>(T, JsonSerializerOptions, CancellationToken)`; the token is now required

### Identity (`N2.Core.Identity`)

- `IAuthenticator.AuthenticateAsync(IUserLogin)` -> `Authenticate(IUserLogin, CancellationToken)`
- `IUserContextFactory.CreateAsync()` -> `Create(CancellationToken)`
- `IIdentityManager.RefreshUserToken(Guid)` -> `RefreshUserToken(Guid, CancellationToken)`
- `IIdentityManager.LogonUser(string, string)` -> `LogonUser(string, string, CancellationToken)`
- `IIdentityManager.LogoffUser(Guid, string)` -> `LogoffUser(Guid, string, CancellationToken)`
- `IIdentityManager.RegisterRefreshToken(Guid, string, string, Guid, DateTime)` -> `RegisterRefreshToken(Guid, string, string, Guid, DateTime, CancellationToken)`
- `IIdentityManager.GetClaims(Guid, string, string)` -> `GetClaims(Guid, string, string, CancellationToken)`
- `IIdentityManager.LogonUserWithSecret(string, string)` -> `LogonUserWithSecret(string, string, CancellationToken)`
- `IIdentityManager.GetUserSecret(string, string)` -> `GetUserSecret(string, string, CancellationToken)`
- `IUserManager<TUser>` (parameters unchanged, token already present): `CanSignInAsync` -> `CanSignIn`,
  `ConfirmEmailAsync` -> `ConfirmEmail`, `CreateAsync` -> `Create`, `CreateRoleAsync` -> `CreateRole`,
  `DeleteAsync` -> `Delete`, `FindByEmailAsync` -> `FindByEmail`, `FindByIdAsync` -> `FindById`,
  `FindByNameAsync` -> `FindByName`, `GenerateConfirmationTokenAsync` -> `GenerateConfirmationToken`,
  `GetRolesAsync` -> `GetRoles`, `GetUserIdAsync` -> `GetUserId`, `IsInRoleAsync` -> `IsInRole`,
  `RemoveFromRoleAsync` -> `RemoveFromRole`, `AddToRoleAsync` -> `AddToRole`, `RemoveRoleAsync` -> `RemoveRole`,
  `RoleExistsAsync` -> `RoleExists`, `SetEmailAsync` -> `SetEmail`, `SetUserNameAsync` -> `SetUserName`,
  `ValidateAsync` -> `Validate`, `ValidateMultifactorAsync` -> `ValidateMultifactor`,
  `SetMultifactorAsync` -> `SetMultifactor`

### Core (`N2.Core`)

- `IAzureFunctionClient.CallAsync<TQ, TA>(string, TQ, CancellationToken)` -> `CallFunction<TQ, TA>(string, TQ, CancellationToken)`
  (not `Call`: that is a reserved keyword in Visual Basic, analyzer CA1716)
- `IEmailService.SendTestEmailAsync()` -> `SendTestEmail(CancellationToken)`
- `IEmailService.SendEmailAsync(string, string, string, string, string)` -> `SendEmail(string, string, string, string, string, CancellationToken)`
- `IEmailService.SendEmailAsync(string, string, string)` -> `SendEmail(string, string, string, CancellationToken)`

## 1.7.1 — 2026-05-10

Optimizations and other minor refactoring; no API surface changes.

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
