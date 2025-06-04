namespace N2.Core.Entity;

/// <summary>
/// A record interface that defines the basic properties of a database record. This interface is used in
/// methods that require a database id, such as the data layer methods for retrieving, updating, or deleting records.
/// </summary>
public interface IDbRecord : IRecord
{
    /// <summary>
    /// Gets or sets the database id.
    /// </summary>
    int Id { get; set; }
}

/// <summary>
/// A record interface that defines the basic properties of a record. This interface is used to
/// transfer data between the data layer and the business layer. It does not contain fields
/// that are specific to the data layer, such as the database id or the database name.
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
