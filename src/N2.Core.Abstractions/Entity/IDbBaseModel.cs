namespace N2.Core.Entity;

/// <summary>
/// Defines the base properties and methods for a database model entity.
/// </summary>
public interface IDbBaseModel
{
    /// <summary>
    /// Gets or sets the primary integer identifier for the entity.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Gets or sets the public GUID identifier for the entity, used for external references.
    /// </summary>
    Guid PublicId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created (UTC).
    /// </summary>
    DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the user who created the entity.
    /// </summary>
    Guid CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is marked as removed (soft delete).
    /// </summary>
    bool IsRemoved { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last modified (UTC).
    /// </summary>
    DateTime Modified { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the user who last modified the entity.
    /// </summary>
    Guid ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was removed, if applicable (UTC).
    /// </summary>
    DateTime? Removed { get; set; }

    /// <summary>
    /// Gets a calculated field for search purposes (concatenated searchable values).
    /// </summary>
    string SearchField { get; }

    /// <summary>
    /// Maps the current entity to a new instance of the specified type. The target is primarily used 
    /// in list views and as such should be rather compact, containing only the essential properties for display purposes.
    /// </summary>
    /// <typeparam name="T">The target type, which must implement <see cref="IBasicListModel"/> and have a parameterless constructor.</typeparam>
    /// <returns>A new instance of type <typeparamref name="T"/> mapped from the current entity.</returns>
    T MapTo<T>() where T : class, IBasicListModel, new();
}
