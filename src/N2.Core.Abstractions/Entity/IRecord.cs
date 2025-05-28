namespace N2.Core.Entity;

public interface IDbRecord : IRecord
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    int Id { get; set; }
}

/// <summary>
/// The record.
/// </summary>
public interface IRecord
{

    /// <summary>
    /// Gets or sets the reference.
    /// </summary>
    Guid Uuid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this record is marked for deletion.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether is the record is locked.
    /// </summary>
    bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets the created date and time.
    /// </summary>
    /// <remarks>Date time value in UTC</remarks>
    DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the last modified date.
    /// </summary>
    /// <remarks>Date time value in UTC</remarks>
    DateTime LastModified { get; set; }
}
