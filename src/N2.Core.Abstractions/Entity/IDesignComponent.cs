using N2.Core.Identity;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace N2.Core.Entity;

public interface IDesignComponent<TList, TDetails>
    where TList : class, IBasicListModel, new()
    where TDetails : class, IBasicListModel, new()
{
    ReadOnlyCollection<TList>? Items { get; }
    Task<RequestResult> SaveItemAsync([NotNull] TDetails model, IUserContext? userContext);
    Task<RequestResult> RemoveItemAsync(Guid publicId, IUserContext? userContext);
    Task<TDetails?> InitializeModelAsync(string? id, IUserContext? userContext);
    Task<bool> LoadItemsAsync(PagingInfo pagingInfo, IUserContext? userContext);
    Task<TDetails?> ReadFromDatabaseAsync(Guid publicId, IUserContext? userContext);
}
