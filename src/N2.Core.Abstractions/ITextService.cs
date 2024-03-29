namespace N2.Core;

public interface ITextService
{
    string GetText(string pageContext, string key);
    string GetGlobalText(string key);
}
