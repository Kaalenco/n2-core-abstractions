namespace N2.Core.Entity;

public interface IBasicListModel : IModifiable
{
    /// <summary>
    /// A name for the item, to be used in drop dowm lists etc.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// A description of the item, to be used for a explanation of the item.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// If the item is active or not. Usage depends on the implementation.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// The identifier for the owner of the item, or empty.
    /// </summary>
    Guid OwnerId { get; set; }

    /// <summary>
    /// The name of the owner of the item, or empty.
    /// </summary>
    string OwnerName { get; set; }

    void Init(string Name, string Description, bool IsActive);
}