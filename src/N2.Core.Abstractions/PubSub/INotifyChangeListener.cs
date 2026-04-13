namespace N2.Core.PubSub;

/// <summary>
/// Listener interface for receiving notifications when items are modified.
/// Implementations should handle notifications quickly to avoid blocking other listeners.
/// </summary>
public interface INotifyChangeListener
{
    /// <summary>
    /// Called when an item has been modified.
    /// This method should return quickly to avoid blocking other notifications.
    /// </summary>
    /// <param name="itemChanged">Information about the modified item.</param>
    void OnItemModified(IItemChanged itemChanged);
}