namespace N2.Core.PubSub;

public interface IItemChanged
{
    DateTime DateTime { get; }
    Type Type { get; }
    Guid Uuid { get; }
}
