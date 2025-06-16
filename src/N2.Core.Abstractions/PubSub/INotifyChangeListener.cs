namespace N2.Core.PubSub;

public interface INotifyChangeListener
{
    void OnItemModified(IItemChanged itemChanged);
}
