namespace N2.Core;

public interface IActivityLogger : IDisposable
{
    void Tag(string tag, object? value);
    void TagError(Exception ex);
}
