namespace N2.Core.Entity;

/// <summary>
/// A base record.
/// </summary>
public abstract class Record : IRecord
{
    /// <summary>
    /// Primary key for the record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// An external reference
    /// </summary>
    public Guid Uuid { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Soft delete flag to indicate removed records
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// A flag indicating that the record is in use / locked.
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last modified date and time.
    /// </summary>
    public DateTime LastModified { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Set the record as modified
    /// </summary>
    public void SetModified()
    {
        LastModified = DateTime.UtcNow;
    }
}