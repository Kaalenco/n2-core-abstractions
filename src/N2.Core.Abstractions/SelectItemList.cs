using System.Collections.ObjectModel;

namespace N2.Core;

public class SelectItemList<T> : Collection<SelectItem<T>>
{
    public SelectItemList()
    {
    }

    public SelectItemList(IEnumerable<(Guid key, T value)> items)
    {
        if (items == null)
        {
            return;
        }

        foreach (var (key, value) in items)
        {
            Add(new SelectItem<T> { Key = key, Value = value });
        }
    }
}
