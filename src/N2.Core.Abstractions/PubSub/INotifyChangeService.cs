using N2.Core.Commands;

namespace N2.Core.PubSub;

public interface INotifyChangeService
{
    void ItemModified<T>(TrackingId trackingId, Guid uuid) where T : class;

    void AddSubscription(INotifyChangeListener listener);

    void RemoveSubscription(INotifyChangeListener listener);
}
