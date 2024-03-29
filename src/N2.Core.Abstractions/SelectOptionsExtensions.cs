namespace N2.Core;

public static class SelectOptionsExtensions
{
    public static bool Ok(this SelectOption option) => option == SelectOption.Ok;
    public static bool Cancel(this SelectOption option) => option == SelectOption.Cancel;
    public static bool Retry(this SelectOption option) => option == SelectOption.Retry;
    public static bool Ignore(this SelectOption option) => option == SelectOption.Ignore;
}