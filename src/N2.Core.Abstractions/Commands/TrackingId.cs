namespace N2.Core.Commands;

public struct TrackingId : IEquatable<TrackingId>
{
    public Guid Value { get; private set; }

    public static TrackingId New()
    {
        return new TrackingId(Guid.NewGuid());
    }

    public TrackingId(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Guid.TryParse(value, out var guidValue))
        {
            throw new ArgumentException("Invalid GUID string.", nameof(value));
        }
        Value = guidValue;
    }

    public TrackingId(Guid value)
    {
        Value = value;
    }

    public static implicit operator Guid(TrackingId d) => d.Value;

    public static implicit operator TrackingId(Guid value) => new(value);

    public bool Equals(TrackingId other) => Value == other.Value;

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is TrackingId other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(TrackingId left, TrackingId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TrackingId left, TrackingId right)
    {
        return !(left == right);
    }

    public Guid ToGuid()
    {
        return Value;
    }

    public static TrackingId ToTrackingId(Guid value)
    {
        return new TrackingId(value);
    }
}