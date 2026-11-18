# Migrating to N2.Core.Abstractions 2.0.0

This guide is for developers upgrading a codebase that consumes `N2.Core.Abstractions` 1.x
(last release 1.7.1) to 2.0.0. The full list of changes is in [CHANGELOG.md](CHANGELOG.md);
this guide explains how to apply them.

If you also use `N2.Core`, upgrade it to 2.0.0 at the same time (it depends on
`N2.Core.Abstractions` 2.0.0) and read the
[N2.Core migration guide](https://dev.azure.com/gjkaal/N2%20Vault%20services/_git/N2.Core?path=/MIGRATION-2.0.md&version=GBtrunk)
for the base classes it ships, such as `BaseCommandHandler`, and the change in its synchronous
command path.

## 1. Summary

**What changed.** Two API conventions, applied to every interface in the package:

- Every member returning `Task`, `ValueTask` or `IAsyncEnumerable` takes a `CancellationToken` as
  its **last** parameter. The token is **required**: there is no default value and no overload
  without it.
- No member name ends in `Async`.

There are no behaviour changes: the package contains only interfaces and plain models, so the
upgrade is a compile-time exercise.

**Who is affected.**

- **Callers** of any asynchronous member on an `N2.Core.Abstractions` interface: rename the call
  and pass a token.
- **Implementers** of those interfaces (repositories, data contexts, HTTP clients, identity and
  user managers, e-mail services, document services, design components): rename the method and
  add the token parameter.
- Code that only uses the synchronous members, models, enums, exceptions or `Encryption` types
  is not affected beyond the retarget.

**Effort.** Mechanical. Each affected call site is a rename plus one extra argument; the compiler
finds all of them (`CS0117`, `CS1061`, `CS7036`, `CS0535`). Expect minutes for a consumer with a
handful of call sites; for a codebase that implements the large interfaces (`IUserManager<TUser>`,
`IIdentityManager`, `IHttpClient`) plan roughly an hour per implementation, most of it spent
passing the token on to the calls inside the implementation.

**Target frameworks.**

| 1.7.1 | 2.0.0 |
| --- | --- |
| `netstandard2.0` | `netstandard2.0` (kept) |
| `netstandard2.1` | `netstandard2.1` (kept) |
| `net8.0` | dropped |
| `net10.0` | `net10.0` |
| -- | `net11.0` (new) |

.NET 8 and .NET 9 applications keep working: they resolve the `netstandard2.1` asset and get
`System.Text.Json`, `System.Text.Encodings.Web` and `Microsoft.Extensions.Primitives` 10.0.15 as
transitive packages.

**Minimum SDK.** The SDK that matches your own target framework: .NET 10 SDK (10.0.x) for
`net10.0`, .NET 11 SDK (11.0.x) for `net11.0`. Projects on .NET 8/9 or .NET Framework consume the
`netstandard` assets and need no new SDK. Building this repository itself needs both the 10.0.x
and 11.0.x SDKs.

## 2. Upgrade checklist

1. **Update the package reference.**

   ```xml
   <PackageReference Include="N2.Core.Abstractions" Version="2.0.0" />
   ```

   With central package management, change the version in `Directory.Packages.props` instead.
   2.0.0 is published to the `vault-packages` Azure Artifacts feed only (see section 4).

2. **Retarget if you were on `net8.0` and want a framework-specific asset.** Change
   `<TargetFramework>` to `net10.0` or `net11.0`, or stay on `net8.0`/`net9.0` and use the
   `netstandard2.1` asset. Install the matching SDK and update `global.json` if you pin one.

3. **Align package versions.** If your project references `System.Text.Json`,
   `System.Text.Encodings.Web`, `Microsoft.Extensions.Primitives` below 10.0.15, or
   `System.IO.Abstractions` below 23.0.1, raise them; otherwise NuGet reports a downgrade
   (`NU1605`). `System.IO.Abstractions` 22 -> 23 is a major version: check its release notes if
   you use its testing helpers directly.

4. **Build and fix the compile errors.** Work through the tables in section 3. For each error:
   - **Caller:** drop the `Async` suffix and add a `CancellationToken` argument.
   - **Implementer:** rename the method, add `CancellationToken cancellationToken` as the last
     parameter, and remove any `= default` you had on an existing token parameter.

5. **Pass real tokens.** Replace any `CancellationToken.None` you added to get it compiling with a
   token from the calling context (section 5). Inside implementations, pass the token on to every
   awaited call that accepts one (EF Core `SaveChangesAsync`, `HttpClient`, `Stream`,
   `Task.Delay`, ...).

6. **Search for leftovers** with the patterns in section 6, then run your tests.

## 3. Breaking changes

Members not listed here are unchanged. "Parameters unchanged" means the member already took a
token in 1.x and only lost the `Async` suffix.

### 3.1 Commands (`N2.Core.Commands`)

| 1.x member | 2.0.0 member |
| --- | --- |
| `ICommandHandler<TQ, TA>.WaitForAsync(TQ)` | `WaitFor(TQ, CancellationToken)` |

`ICommandHandler.Invoke(ICommandRequest)` and `ICommandHandler<TQ>.ExecuteCommand(TQ)` are
synchronous and unchanged. Most handlers derive from `N2.Core.BaseCommandHandler<TQ, TA>`, which
implements `WaitFor`; see the N2.Core guide for the `HandleRequest` override.

```csharp
// 1.x
TokenResponse response = await handler.WaitForAsync(request);

// 2.0.0
TokenResponse response = await handler.WaitFor(request, cancellationToken);
```

### 3.2 Entity (`N2.Core.Entity`)

| 1.x member | 2.0.0 member |
| --- | --- |
| `IUnitOfWork.Complete()` | `Complete(CancellationToken)` |
| `ICoreDataContext.DeleteAsync<T>(Guid)` | `Delete<T>(Guid, CancellationToken)` |
| `ICoreDataContext.FindRecordAsync<T>(Guid)` | `FindRecord<T>(Guid, CancellationToken)` |
| `ICoreDataContext.GetSelectListAsync(string)` | `GetSelectList(string, CancellationToken)` |
| `ICoreDataContextFactory<TContext>.CreateAsync(string)` | `Create(string, CancellationToken)` |
| `ICoreDataContextFactory<TContext>.CreateAsync()` | `Create(CancellationToken)` |
| `IDataRepository<T>.CreateAndReturn(T)` | `CreateAndReturn(T, CancellationToken)` |
| `IDataRepository<T>.UpdateAndReturn(int, T)` | `UpdateAndReturn(int, T, CancellationToken)` |
| `IDesignComponent<TList, TDetails>.SaveItemAsync(TDetails, IUserContext?)` | `SaveItem(TDetails, IUserContext?, CancellationToken)` |
| `IDesignComponent<TList, TDetails>.RemoveItemAsync(Guid, IUserContext?)` | `RemoveItem(Guid, IUserContext?, CancellationToken)` |
| `IDesignComponent<TList, TDetails>.InitializeModelAsync(string?, IUserContext?)` | `InitializeModel(string?, IUserContext?, CancellationToken)` |
| `IDesignComponent<TList, TDetails>.LoadItemsAsync(PagingInfo, IUserContext?)` | `LoadItems(PagingInfo, IUserContext?, CancellationToken)` |
| `IDesignComponent<TList, TDetails>.ReadFromDatabaseAsync(Guid, IUserContext?)` | `ReadFromDatabase(Guid, IUserContext?, CancellationToken)` |

`IUnitOfWork.Complete` and `IDataRepository<T>.CreateAndReturn`/`UpdateAndReturn` had no `Async`
suffix in 1.x, so a text search for `Async(` does not find them; search for `.Complete()` and the
method names instead.

Implementing `IUnitOfWork` (EF Core):

```csharp
// 1.x
public async Task<(ResponseStatus status, string? message)> Complete()
{
    await dbContext.SaveChangesAsync();
    return (ResponseStatus.Success, null);
}

// 2.0.0
public async Task<(ResponseStatus status, string? message)> Complete(CancellationToken cancellationToken)
{
    await dbContext.SaveChangesAsync(cancellationToken);
    return (ResponseStatus.Success, null);
}
```

Calling it from a service:

```csharp
// 1.x
repository.Add(item);
(ResponseStatus status, string? message) = await unitOfWork.Complete();

// 2.0.0
repository.Add(item);
(ResponseStatus status, string? message) = await unitOfWork.Complete(cancellationToken);
```

Implementing `ICoreDataContextFactory<TContext>`:

```csharp
// 2.0.0
public Task<AppDataContext> Create(CancellationToken cancellationToken)
    => Create(DefaultConnectionName, cancellationToken);

public async Task<AppDataContext> Create(string connectionName, CancellationToken cancellationToken)
{
    AppDataContext context = new(optionsFor(connectionName));
    await context.Database.OpenConnectionAsync(cancellationToken);
    return context;
}
```

### 3.3 Document management (`N2.Core.Dms`)

| 1.x member | 2.0.0 member |
| --- | --- |
| `IDocumentRepository.CompleteAsync(IUserContext, CancellationToken)` | `Complete(IUserContext, CancellationToken)` (parameters unchanged) |
| `IDocumentRepository.FindDocumentAsync(...)` (4 overloads) | `FindDocument(...)` (parameters unchanged) |
| `IDocumentService.FindDocumentsAsync(string, IEnumerable<string>, string, bool)` | `FindDocuments(string, IEnumerable<string>, string, bool, CancellationToken)` |
| `IDocumentService.DeleteDocumentAsync(Guid)` | `DeleteDocument(Guid, CancellationToken)` |
| `IDocumentService.SaveDocumentAsync(Stream, DocumentInformation)` | `SaveDocument(Stream, DocumentInformation, CancellationToken)` |
| `IDocumentService.UpdateDocumentAsync(Guid, DocumentInformation)` | `UpdateDocument(Guid, DocumentInformation, CancellationToken)` |
| `IDocumentService.GetDocumentInformationAsync(Guid)` | `GetDocumentInformation(Guid, CancellationToken)` |

`IDocumentRepository.RemoveDocument(Guid, CancellationToken)` is unchanged.

```csharp
// 1.x
(bool success, DocumentInformation document) = await documentService.SaveDocumentAsync(stream, info);

// 2.0.0
(bool success, DocumentInformation document) = await documentService.SaveDocument(stream, info, cancellationToken);
```

### 3.4 HTTP (`N2.Core.Http`)

`IHttpClient` (parameters unchanged; every member already took a token):

| 1.x member | 2.0.0 member |
| --- | --- |
| `DeleteAsync(string, CancellationToken)` | `Delete(string, CancellationToken)` |
| `GetRelativeAsync<TResponse>(string, CancellationToken)` | `GetRelative<TResponse>(string, CancellationToken)` |
| `ReadJsonDocumentAsync(Uri, CancellationToken)` | `ReadJsonDocument(Uri, CancellationToken)` |
| `GetFromUriAsync<TResource>(Uri, CancellationToken)` | `GetFromUri<TResource>(Uri, CancellationToken)` |
| `PostRelativeAsync<TRequest, TResponse>(string, TRequest, CancellationToken)` | `PostRelative<TRequest, TResponse>(string, TRequest, CancellationToken)` |
| `PostResourceAsync<TResource>(Uri, TResource, CancellationToken)` | `PostResource<TResource>(Uri, TResource, CancellationToken)` |
| `PostAsync(Uri, HttpContent, CancellationToken)` | `Post(Uri, HttpContent, CancellationToken)` |
| `GetAsync(Uri, CancellationToken)` | **`GetResponseMessage(Uri, CancellationToken)`** (see 3.7) |
| `PutRelativeAsync<TRequest, TResponse>(string, TRequest, CancellationToken)` | `PutRelative<TRequest, TResponse>(string, TRequest, CancellationToken)` |
| `PutResourceAsync<TResource>(Uri, TResource, CancellationToken)` | `PutResource<TResource>(Uri, TResource, CancellationToken)` |

`IHttpRequest` / `IHttpResponse` (the optional token became required):

| 1.x member | 2.0.0 member |
| --- | --- |
| `IHttpRequest.ReadAsStringAsync(CancellationToken = default)` | `ReadAsString(CancellationToken)` |
| `IHttpResponse.WriteAsync(string, CancellationToken = default)` | `Write(string, CancellationToken)` |
| `IHttpResponse.WriteAsync(byte[], CancellationToken = default)` | `Write(byte[], CancellationToken)` |
| `IHttpResponse.WriteAsJsonAsync<T>(T, JsonSerializerOptions, CancellationToken = default)` | `WriteAsJson<T>(T, JsonSerializerOptions, CancellationToken)` |

Calling `IHttpClient.GetResponseMessage`:

```csharp
// 1.x
using HttpResponseMessage response = await client.GetAsync(uri, token);

// 2.0.0
using HttpResponseMessage response = await client.GetResponseMessage(uri, token);
if (!response.IsSuccessStatusCode) { /* read status and headers */ }
```

Implementing `IHttpClient` on top of `System.Net.Http.HttpClient` -- note that the inner calls keep
their BCL `Async` names; only the members of the N2 interface are renamed:

```csharp
// 1.x
public Task<HttpResponseMessage> GetAsync(Uri resourcePath, CancellationToken token)
    => httpClient.GetAsync(resourcePath, token);

public Task<HttpResponseMessage> PostAsync(Uri resourcePath, HttpContent content, CancellationToken token)
    => httpClient.PostAsync(resourcePath, content, token);

// 2.0.0
public Task<HttpResponseMessage> GetResponseMessage(Uri resourcePath, CancellationToken token)
    => httpClient.GetAsync(resourcePath, token);

public Task<HttpResponseMessage> Post(Uri resourcePath, HttpContent content, CancellationToken token)
    => httpClient.PostAsync(resourcePath, content, token);
```

Callers of `IHttpRequest`/`IHttpResponse` that relied on the default token must now pass one:

```csharp
// 1.x
string body = await request.ReadAsStringAsync();
await response.WriteAsJsonAsync(result, options);

// 2.0.0
string body = await request.ReadAsString(cancellationToken);
await response.WriteAsJson(result, options, cancellationToken);
```

### 3.5 Identity (`N2.Core.Identity`)

| 1.x member | 2.0.0 member |
| --- | --- |
| `IAuthenticator.AuthenticateAsync(IUserLogin)` | `Authenticate(IUserLogin, CancellationToken)` |
| `IUserContextFactory.CreateAsync()` | `Create(CancellationToken)` |
| `IIdentityManager.RefreshUserToken(Guid)` | `RefreshUserToken(Guid, CancellationToken)` |
| `IIdentityManager.LogonUser(string, string)` | `LogonUser(string, string, CancellationToken)` |
| `IIdentityManager.LogoffUser(Guid, string)` | `LogoffUser(Guid, string, CancellationToken)` |
| `IIdentityManager.RegisterRefreshToken(Guid, string, string, Guid, DateTime)` | `RegisterRefreshToken(Guid, string, string, Guid, DateTime, CancellationToken)` |
| `IIdentityManager.GetClaims(Guid, string, string)` | `GetClaims(Guid, string, string, CancellationToken)` |
| `IIdentityManager.LogonUserWithSecret(string, string)` | `LogonUserWithSecret(string, string, CancellationToken)` |
| `IIdentityManager.GetUserSecret(string, string)` | `GetUserSecret(string, string, CancellationToken)` |

`IIdentityManager` members had no `Async` suffix in 1.x; only the token is new.

`IUserManager<TUser>`: parameters unchanged (every member already took a token), only the suffix
is removed:

| 1.x | 2.0.0 | 1.x | 2.0.0 |
| --- | --- | --- | --- |
| `CanSignInAsync` | `CanSignIn` | `GetUserIdAsync` | `GetUserId` |
| `ConfirmEmailAsync` | `ConfirmEmail` | `IsInRoleAsync` | `IsInRole` |
| `CreateAsync` | `Create` | `RemoveFromRoleAsync` | `RemoveFromRole` |
| `CreateRoleAsync` | `CreateRole` | `AddToRoleAsync` | `AddToRole` |
| `DeleteAsync` | `Delete` | `RemoveRoleAsync` | `RemoveRole` |
| `FindByEmailAsync` | `FindByEmail` | `RoleExistsAsync` | `RoleExists` |
| `FindByIdAsync` | `FindById` | `SetEmailAsync` | `SetEmail` |
| `FindByNameAsync` | `FindByName` | `SetUserNameAsync` | `SetUserName` |
| `GenerateConfirmationTokenAsync` | `GenerateConfirmationToken` | `ValidateAsync` | `Validate` |
| `GetRolesAsync` | `GetRoles` | `ValidateMultifactorAsync` | `ValidateMultifactor` |
| | | `SetMultifactorAsync` | `SetMultifactor` |

Implementing `IIdentityManager`:

```csharp
// 1.x
public async Task<Guid> LogonUser(string userName, string password)
{
    User? user = await users.FindByNameAsync(userName);
    ...
}

// 2.0.0
public async Task<Guid> LogonUser(string userName, string password, CancellationToken cancellationToken)
{
    User? user = await users.FindByNameAsync(userName, cancellationToken);
    ...
}
```

An `IUserManager<TUser>` implementation that wraps ASP.NET Core Identity's `UserManager<TUser>`
keeps calling the framework's `...Async` methods inside; only the outer names change:

```csharp
// 2.0.0
public async Task<ICommandResponse<TUser>> FindByEmail(string emailAddress, CancellationToken token)
{
    token.ThrowIfCancellationRequested();
    TUser? user = await userManager.FindByEmailAsync(emailAddress);
    ...
}
```

### 3.6 Core (`N2.Core`)

| 1.x member | 2.0.0 member |
| --- | --- |
| `IAzureFunctionClient.CallAsync<TQ, TA>(string, TQ, CancellationToken)` | **`CallFunction<TQ, TA>(string, TQ, CancellationToken)`** (see 3.7) |
| `IEmailService.SendTestEmailAsync()` | `SendTestEmail(CancellationToken)` |
| `IEmailService.SendEmailAsync(string, string, string, string, string)` | `SendEmail(string, string, string, string, string, CancellationToken)` |
| `IEmailService.SendEmailAsync(string, string, string)` | `SendEmail(string, string, string, CancellationToken)` |

```csharp
// 1.x
OrderResult? result = await functionClient.CallAsync<OrderRequest, OrderResult>("api/orders", request, token);
await emailService.SendEmailAsync(recipient, subject, html);

// 2.0.0
OrderResult? result = await functionClient.CallFunction<OrderRequest, OrderResult>("api/orders", request, token);
await emailService.SendEmail(recipient, subject, html, cancellationToken);
```

### 3.7 Why `GetResponseMessage` and `CallFunction`

Dropping the suffix would have produced `IHttpClient.Get` and `IAzureFunctionClient.Call`. `Get`
and `Call` are reserved keywords in Visual Basic, and the package builds with all .NET analyzers
enabled as errors, so analyzer **CA1716** ("Identifiers should not match keywords") rejects them:
a member with a keyword name is awkward to call from another .NET language. Instead of
suppressing the rule, the members got descriptive names:

- `IHttpClient.GetAsync` -> `GetResponseMessage`: it returns the raw `HttpResponseMessage`
  without deserializing, which the name now says (compare `GetRelative<T>` / `GetFromUri<T>`,
  which deserialize).
- `IAzureFunctionClient.CallAsync` -> `CallFunction`: it calls an Azure Function.

These are the only two members whose new name is not "old name minus `Async`".

## 4. Behaviour, package and dependency changes

**Behaviour.** None. Only signatures changed. The token is a contract for implementers: an
implementation should pass it on and may throw `OperationCanceledException` when it is cancelled.

**Dependencies.**

| Package | 1.7.1 | 2.0.0 |
| --- | --- | --- |
| `System.IO.Abstractions` | 22.1.0 | 23.0.1 (all targets) |
| `System.Text.Json` | 10.0.5 (all targets) | 10.0.15, `netstandard` targets only (in-box on net10.0+) |
| `System.Text.Encodings.Web` | 10.0.5 (all targets) | 10.0.15, `netstandard` targets only (in-box on net10.0+) |
| `Microsoft.Extensions.Primitives` | 10.0.5 (all targets) | 10.0.15 for `netstandard` and `net10.0`; none for `net11.0` (in-box in `Microsoft.NETCore.App`) |

- The out-of-band packages are pinned to the **10.0.x band**, not 11.0.0, so a `net10.0`
  application on the ASP.NET Core 10 shared framework is not lifted to 11.0.0 assemblies.
- On `net11.0` there is no package reference for `Microsoft.Extensions.Primitives` because it is
  part of the shared framework there; a direct reference would be pruned and raise `NU1510`.
  If your own `net11.0` project references it explicitly, you can remove that reference too.

**Publishing.** 2.0.0 is published to the `vault-packages` Azure Artifacts feed only. A nuget.org
release is deferred (nuget.org recommends Trusted Publishing, which needs GitHub Actions). Make
sure the consuming solution's `nuget.config` has the `vault-packages` source.

## 5. Where to get a CancellationToken

Take the token from the outermost caller and pass it down through every layer. Add a
`CancellationToken cancellationToken` parameter (last, no default) to your own async methods
rather than creating tokens in the middle of a call chain.

**ASP.NET Core minimal API or controller.** Add a `CancellationToken` parameter; it binds to
`HttpContext.RequestAborted` and is cancelled when the client disconnects.

```csharp
app.MapGet("/documents/{id:guid}", async (Guid id, IDocumentService documents, CancellationToken cancellationToken) =>
{
    (bool success, DocumentInformation document) = await documents.GetDocumentInformation(id, cancellationToken);
    return success ? Results.Ok(document) : Results.NotFound();
});

[HttpPost]
public async Task<IActionResult> Save(OrderDto dto, CancellationToken cancellationToken)
{
    ...
    await unitOfWork.Complete(cancellationToken);
}
```

Where no parameter is available (middleware, filters), use `HttpContext.RequestAborted`.

**Blazor component.** Components have no built-in token; own a `CancellationTokenSource` and
cancel it on disposal, so work for a component that has been navigated away from stops:

```razor
@implements IDisposable

@code {
    private readonly CancellationTokenSource disposalSource = new();

    protected override async Task OnInitializedAsync()
        => loaded = await designComponent.LoadItems(paging, userContext, disposalSource.Token);

    public void Dispose()
    {
        disposalSource.Cancel();
        disposalSource.Dispose();
    }
}
```

**Background service.** Use the `stoppingToken` of `BackgroundService.ExecuteAsync`, or the token
passed to `IHostedService.StartAsync`/`StopAsync`.

```csharp
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        await emailService.SendTestEmail(stoppingToken);
        await Task.Delay(interval, stoppingToken);
    }
}
```

**Azure Functions (isolated worker).** Add a `CancellationToken` parameter to the function, or
use `FunctionContext.CancellationToken`.

**Tests.** Use the test framework's token so a hanging test is cancelled on timeout or abort:

- MSTest: `TestContext.CancellationToken` (inject `TestContext` through the test class
  constructor or the `TestContext` property);
- NUnit: `TestContext.CurrentContext.CancellationToken`;
- xUnit v3: `TestContext.Current.CancellationToken`.

When setting up mocks, match the new parameter with `It.IsAny<CancellationToken>()` (Moq):

```csharp
unitOfWork.Setup(u => u.Complete(It.IsAny<CancellationToken>()))
          .ReturnsAsync((ResponseStatus.Success, (string?)null));
```

**Avoid `CancellationToken.None`** except at a true top-level entry point that has no token to
offer (a `Main` without a host, a fire-and-forget migration step, a synchronous legacy entry
point). Passing `None` deeper in the stack compiles, but silently opts that call out of
cancellation: a request the client has abandoned keeps running against the database. If a call
must finish even when the caller cancels (for example, writing an audit record), make that an
explicit, commented decision.

## 6. Finding affected call sites

The compiler finds every break, but these searches help to scope the work up front and to catch
leftover `CancellationToken.None` or `default` tokens afterwards. Patterns are regular expressions
(for example `git grep -nE '<pattern>' -- '*.cs' '*.razor'`). BCL and EF Core methods legitimately
keep their `Async` suffix, so expect hits that need no change.

| Pattern | Finds |
| --- | --- |
| `Async\(` | any former N2 `...Async(` call or implementation (filter out BCL/EF calls) |
| `Async<` | generic former N2 members such as `GetRelativeAsync<T>`, `CallAsync<TQ, TA>`, `DeleteAsync<T>` |
| `\.Complete\(\)` | `IUnitOfWork.Complete()` calls without a token |
| `CompleteAsync\(` | `IDocumentRepository.CompleteAsync` |
| `\.WaitForAsync\(` | `ICommandHandler<TQ, TA>.WaitForAsync` |
| `\bGetAsync\(` | `IHttpClient.GetAsync` -> `GetResponseMessage` (check the receiver type) |
| `\bCallAsync\b` | `IAzureFunctionClient.CallAsync` -> `CallFunction` |
| `(CreateAndReturn\|UpdateAndReturn)\(` | `IDataRepository<T>` calls, no suffix in 1.x |
| `(LogonUser\|LogonUserWithSecret\|LogoffUser\|RefreshUserToken\|RegisterRefreshToken\|GetClaims\|GetUserSecret)\(` | `IIdentityManager` calls, no suffix in 1.x |
| `(ReadAsStringAsync\|WriteAsJsonAsync\|WriteAsync)\(` | `IHttpRequest` / `IHttpResponse` calls that relied on the default token |
| `CancellationToken\s+\w+\s*=\s*default` | implementations that still declare an optional token |
| `CancellationToken\.None\|default\(CancellationToken\)` | placeholder tokens to replace (section 5) |
| `<TargetFrameworks?>.*net8\.0` | projects still targeting `net8.0` |
