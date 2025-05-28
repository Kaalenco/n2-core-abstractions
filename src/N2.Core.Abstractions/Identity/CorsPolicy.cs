namespace N2.Core.Identity;

/// <summary>
/// The cors policy for configuration.
/// </summary>

public class CorsPolicy
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Gets or sets the origins.
    /// </summary>
    public string[]? Origins { get; set; }
}
