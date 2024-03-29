namespace N2.Core;

public enum ErrorCode
{
    None,
    ValueNullOrEmpty,
    StartLowerThenEnd,
    MinimumValue,
    StringLenght
}

public class ErrorCodeDescription
{
    public string Code { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
}