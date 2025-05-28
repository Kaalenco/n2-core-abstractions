namespace N2.Core.Commands;

/// <summary>
/// The dictionary response.
/// </summary>
public class DictionaryResponse : CommandResponse<Dictionary<string, object>>, ICommandResponse<Dictionary<string, object>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryResponse" /> class.
    /// </summary>
    public DictionaryResponse()
    {
        Value = [];
    }
}