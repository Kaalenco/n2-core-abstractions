namespace N2.Core.PubSub;

/// <summary>
/// Information about a modified item
/// </summary>
public struct ItemChanged : IItemChanged, IEquatable<ItemChanged>
{
    public DateTime DateTime { get; set; }
    public Type Type { get; set; }
    public Guid Uuid { get; set; }
    public static bool operator !=(ItemChanged left, ItemChanged right)
    {
        return !(left == right);
    }

    public static bool operator ==(ItemChanged left, ItemChanged right)
    {
        return left.Equals(right);
    }

    public readonly override bool Equals(object? obj)
    {
        return (obj is ItemChanged itemChanged)
        ? Equals(itemChanged)
        : false;
    }

    public readonly bool Equals(ItemChanged other) {
        return
            Type == other.Type &&
            Uuid == other.Uuid;
    }

    public override int GetHashCode()
    {
#if NET8_0_OR_GREATER
        return HashCode.Combine(Type, Uuid);
#else
        unchecked
        {
            // Use unchecked to avoid overflow exceptions
            // use prime numbers to reduce collisions
            // 17	19	23	29	31	37	41	43	47	53	59	61	67	71
            int hash = 17; // use prime numbers
            hash = hash * 31 + (Type?.GetHashCode() ?? 0);
            hash = hash * 47 + Uuid.GetHashCode();
            return hash;
        }
#endif
    }
}