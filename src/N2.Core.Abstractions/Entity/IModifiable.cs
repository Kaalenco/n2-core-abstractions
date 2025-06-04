namespace N2.Core.Entity;

/// <summary>
/// Defines properties for entities that support modification tracking.
/// This interface is typically used for entities that need to track creation and modification metadata
/// but don't require the full database entity lifecycle tracking of <see cref="IDbBaseModel"/>.
/// </summary>
public interface IModifiable
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity is a database item.
    /// This flag can be used to distinguish between in-memory and persisted entities.
    /// </summary>
    bool DbItem { get; set; }

    /// <summary>
    /// Gets or sets the public GUID identifier for the entity, used for external references.
    /// This identifier remains consistent across different environments and database instances.
    /// </summary>
    Guid PublicId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created (UTC).
    /// This value should not change after initial creation.
    /// </summary>
    DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last modified (UTC).
    /// This value should be updated whenever the entity is changed.
    /// </summary>
    DateTime Modified { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the user who created the entity.
    /// This allows for auditing of entity creation.
    /// </summary>
    Guid CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the GUID of the user who last modified the entity.
    /// This allows for auditing of entity modifications.
    /// </summary>
    Guid ModifiedBy { get; set; }
}
