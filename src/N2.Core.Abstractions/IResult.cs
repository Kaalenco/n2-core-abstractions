namespace N2.Core;

public interface IResult<T>
{
    ResponseType Type { get; }
    bool Success { get; }
    string Message { get; }
    T Value { get; }

    void Deconstruct(out T Value, out ResponseType Type);
    bool Equals(object? obj);
    bool Equals(IResult<T>? other);
    int GetHashCode();
    string ToString();
}
