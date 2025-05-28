namespace N2.Core.Commands;

public interface IListResponse<T> : ICommandResponse<IReadOnlyList<T>>
{
}

public class ListResponse<T> : CommandResponse<IReadOnlyList<T>>, IListResponse<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryResponse"/> class.
    /// </summary>
    public ListResponse()
    {
        Value = [];
    }

    public ListResponse(IEnumerable<T> values)
    {
        Value = [.. values];
    }
}
