namespace N2.Core.PubSub;

/// <summary>
/// Represents metadata about an item that has been modified.
/// </summary>
public interface IItemChanged
{
    /// <summary>
    /// Gets the timestamp when the modification occurred.
    /// </summary>
    DateTime DateTime { get; }

    /// <summary>
    /// Gets the type of the modified item.
    /// </summary>
    Type Type { get; }

    /// <summary>
    /// Gets the unique identifier of the modified item.
    /// </summary>
    Guid Uuid { get; }
}