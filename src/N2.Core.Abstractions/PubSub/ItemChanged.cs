namespace N2.Core.PubSub;

/// <summary>
/// Information about a modified item.
/// Equality is based on Type and Uuid only; DateTime is not considered.
/// </summary>
public readonly struct ItemChanged : IItemChanged, IEquatable<ItemChanged>
{
    public ItemChanged(Type type, Guid uuid)
    {
        DateTime = DateTime.UtcNow;
        Type = type;
        Uuid = uuid;
    }

    public ItemChanged(DateTime dateTime, Type type, Guid uuid)
    {
        DateTime = dateTime;
        Type = type;
        Uuid = uuid;
    }

    /// <summary>
    /// Gets the timestamp when the item was modified.
    /// Note: Not used in equality comparison.
    /// </summary>
    public DateTime DateTime { get; }
    
    /// <summary>
    /// Gets the type of the modified item.
    /// </summary>
    public Type Type { get; }
    
    /// <summary>
    /// Gets the unique identifier of the modified item.
    /// </summary>
    public Guid Uuid { get; }
    
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
            hash = hash * 31 + Type.GetHashCode();
            hash = hash * 47 + Uuid.GetHashCode();
            return hash;
        }
#endif
    }
}