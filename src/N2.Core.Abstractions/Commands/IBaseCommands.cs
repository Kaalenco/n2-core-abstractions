
namespace N2.Core.Commands;

/// <summary>
/// Defines a set of basic command operations for managing view or DTO models of type <typeparamref name="TModel"/>.
/// These commands are intended for use with front-end or command processor interactions, not for direct database entities.
/// The <see cref="TrackingId"/> parameter is used for logging and tracing, not for entity generation or updates.
/// </summary>
/// <typeparam name="TModel">
/// The model type exchanged with the front end or command processor.
/// This should be a view or DTO, not a database entity.
/// </typeparam>
public interface IBaseCommands<TModel> where TModel : new()
{
    /// <summary>
    /// Generates a unique code for a new model instance.
    /// </summary>
    /// <param name="trackingKey">Used for logging and tracing.</param>
    /// <returns>A unique code as a string.</returns>
    string GenerateFreeCode(TrackingId trackingKey);

    /// <summary>
    /// Checks if a model with the specified code exists.
    /// </summary>
    CommandResponse Exists(TrackingId trackingKey, string code);

    /// <summary>
    /// Checks if a model with the specified unique identifier exists.
    /// </summary>
    CommandResponse Exists(TrackingId trackingKey, Guid uuid);

    /// <summary>
    /// Deletes the model with the specified unique identifier.
    /// </summary>
    CommandResponse Delete(TrackingId trackingKey, Guid uuid);

    /// <summary>
    /// Returns a collection of reference items, optionally filtered.
    /// </summary>
    IEnumerable<TModel> ReferenceItems(string? find);

    /// <summary>
    /// Finds items and passes the results to the provided collector action.
    /// </summary>
    CommandResponse FindItems(TrackingId trackingKey, string? find, Action<IEnumerable<TModel>>? collector);

    /// <summary>
    /// Retrieves a model by its code and passes it to the provided action if found.
    /// </summary>
    CommandResponse ByCode(TrackingId trackingKey, string code, Action<TModel>? findItem);

    /// <summary>
    /// Retrieves a model by its unique identifier and passes it to the provided action if found.
    /// </summary>
    CommandResponse ById(TrackingId trackingKey, Guid uuid, Action<TModel>? findItem);

    /// <summary>
    /// Adds a new model or updates an existing one, and optionally processes the result with the provided action.
    /// </summary>
    CommandResponse AddOrUpdate(TrackingId trackingKey, TModel? record, Action<TModel?>? findItem = null);

    /// <summary>
    /// Adds a new model if one with the specified unique identifier does not already exist.
    /// </summary>
    /// <returns>True if the model was added; false if it already exists.</returns>
    bool AddIfNotExists(TrackingId trackingId, Guid uuid, string name, string code);
}
