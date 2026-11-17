using System.Collections.ObjectModel;

using N2.Core.Identity;

namespace N2.Core.Entity;

/// <summary>
/// Defines a generic component for managing data entities in a design-oriented context.
/// This interface provides standardized methods for CRUD operations on entity models,
/// with support for user context-based permissions and paginated data loading.
/// </summary>
/// <typeparam name="TList">The list view model type, representing simplified entity data for listings.</typeparam>
/// <typeparam name="TDetails">The detailed view model type, representing complete entity data for editing.</typeparam>
public interface IDesignComponent<TList, TDetails>
    where TList : class, IBasicListModel, new()
    where TDetails : class, IBasicListModel, new()
{
    /// <summary>
    /// Gets the current collection of list items managed by this component.
    /// </summary>
    /// <remarks>
    /// This collection typically provides a read-only view of all managed
    /// entities in their list representation. It should be limited to about 100 items for performance reasons.
    /// For larger datasets, use the <see cref="LoadItems"/> method to load items in a paginated manner.
    /// </remarks>
    ReadOnlyCollection<TList>? Items { get; }

    /// <summary>
    /// Persists an entity model to the underlying data store.
    /// </summary>
    /// <param name="model">The model to save. Cannot be null.</param>
    /// <param name="userContext">The current user context for permission checking and auditing. Can be null.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="RequestResult"/> indicating the outcome of the save operation,
    /// including success status and any relevant messages.
    /// </returns>
    Task<RequestResult> SaveItem(TDetails model, IUserContext? userContext, CancellationToken cancellationToken);

    /// <summary>
    /// Removes an entity from the underlying data store.
    /// </summary>
    /// <param name="publicId">The unique public identifier of the entity to remove.</param>
    /// <param name="userContext">The current user context for permission checking and auditing. Can be null.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="RequestResult"/> indicating the outcome of the remove operation,
    /// including success status and any relevant messages.
    /// </returns>
    /// <remarks>
    /// If the entity cannot be found or the user lacks permission to remove it, this method will return a failure result.
    /// </remarks>
    Task<RequestResult> RemoveItem(Guid publicId, IUserContext? userContext, CancellationToken cancellationToken);

    /// <summary>
    /// Initializes a new entity model or loads an existing one based on the provided identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to load, or null to initialize a new entity.</param>
    /// <param name="userContext">The current user context for permission checking. Can be null.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// The initialized or loaded entity model, or null if initialization fails or the entity cannot be found.
    /// </returns>
    Task<TDetails?> InitializeModel(string? id, IUserContext? userContext, CancellationToken cancellationToken);

    /// <summary>
    /// Loads entities into the <see cref="Items"/> collection based on paging, sorting, and search criteria.
    /// </summary>
    /// <param name="pagingInfo">
    /// Information about the page to load, including page number, page size, search query, and sort options.
    /// </param>
    /// <param name="userContext">The current user context for permission checking. Can be null.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// True if the load operation was successful; otherwise, false.
    /// </returns>
    Task<bool> LoadItems(PagingInfo pagingInfo, IUserContext? userContext, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a specific entity from the underlying data store by its unique public identifier.
    /// </summary>
    /// <param name="publicId">The unique public identifier of the entity to retrieve.</param>
    /// <param name="userContext">The current user context for permission checking. Can be null.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// The retrieved entity model, or null if the entity cannot be found or the user lacks permission.
    /// </returns>
    Task<TDetails?> ReadFromDatabase(Guid publicId, IUserContext? userContext, CancellationToken cancellationToken);
}
