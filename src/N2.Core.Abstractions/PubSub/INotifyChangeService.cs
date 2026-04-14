using N2.Core.Commands;

namespace N2.Core.PubSub;

/// <summary>
/// Service for notifying listeners about item modifications in a publish-subscribe pattern.
/// Implementations should be thread-safe for concurrent subscriptions and notifications.
/// </summary>
public interface INotifyChangeService
{
    /// <summary>
    /// Notifies all registered listeners that an item has been modified.
    /// </summary>
    /// <typeparam name="T">The type of item that implements <see cref="IItemChanged"/>.</typeparam>
    /// <param name="trackingId">Correlation identifier for tracking this notification across systems.</param>
    /// <param name="item">The modified item to notify listeners about.</param>
    /// <returns>The number of listeners that processed the notification.</returns>
    int ItemModified<T>(TrackingId trackingId, T item) where T : IItemChanged;

    /// <summary>
    /// Registers a listener to receive notifications when items are modified.
    /// This method should be thread-safe.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    /// <returns><see langword="true"/> if the listener was added; <see langword="false"/> if it was already registered.</returns>
    bool AddSubscription(INotifyChangeListener listener);

    /// <summary>
    /// Unregisters a listener from receiving notifications.
    /// This method should be thread-safe.
    /// </summary>
    /// <param name="listener">The listener to remove.</param>
    /// <returns><see langword="true"/> if the listener was found and removed; <see langword="false"/> if it was not registered.</returns>
    bool RemoveSubscription(INotifyChangeListener listener);
}