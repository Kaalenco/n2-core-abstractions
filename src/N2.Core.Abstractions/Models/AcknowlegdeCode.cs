namespace N2.Core.Models;

/// <summary>
/// Enum for the different response codes.
/// </summary>
[Flags]
public enum AcknowledgeCodes
{
    None = 0,
    Yes = 1,
    No = 2,
    YesNo = 3,
    Ok = 4,
    Cancel = 8,
    OkCancel = 12,
    Retry = 16,
    CancelRetry = 24,
    Abort = 32,
    RetryAbort = 48,
    YesNoCancel = 14,
    Error = 64,
    YesNoRetry = 19
}
