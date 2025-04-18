namespace N2.Core.Entity;

public abstract class BasicListModel : IBasicListModel
{
    /// <summary>
    /// A name for the item, to be used in drop dowm lists etc.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// A description of the item, to be used for a explanation of the item.
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// If the item is active or not. Usage depends on the implementation.
    /// </summary>
    public abstract bool IsActive { get; }

    /// <summary>
    /// The identifier for the owner of the item, or empty.
    /// </summary>
    public Guid OwnerId { get; set; } = Guid.Empty;

    /// <summary>
    /// The name for the owner of the item, or empty.
    /// </summary>
    public string OwnerName { get; set; } = string.Empty;

    // DbItem is set when the model is retrieved from the database
    // and is used to determine if the model is new or existing
    public bool DbItem { get; set; }

    public Guid PublicId { get; set; } = Guid.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Modified { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; } = Guid.Empty;
    public Guid ModifiedBy { get; set; } = Guid.Empty;

    public abstract bool IsValid();
    public abstract void Init(string Name, string Description, bool IsActive);

    // Feedback from the server to the client after a request
    public string ValidationMessage { get; set; } = string.Empty;
    public RequestResult UpdateResult { get; protected set; }
}
