namespace N2.Core;

public class SelectItem
{
    public Guid Key { get; set; } = Guid.Empty;
    public string? Group { get; set; }
    public bool Disabled { get; set; }
}

public class SelectItem<T> : SelectItem
{
    public T? Value { get; set; }

    public SelectItem()
    {
        Value = default;
    }

    public SelectItem(T value)
    {
        Value = value;
    }

    public SelectItem(Guid key, T value)
    {
        Key = key;
        Value = value;
    }
}
