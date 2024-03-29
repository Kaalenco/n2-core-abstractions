namespace N2.Core;

public sealed class ValidationResult
{
    public string Message { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
    public ErrorCode ErrorCode { get; set; }
}
