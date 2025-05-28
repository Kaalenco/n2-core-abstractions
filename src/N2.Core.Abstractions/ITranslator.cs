namespace N2.Core;

public interface ITranslator
{
    string Language { get; }
    string Translate(string language, string pageContext, string key);
    string Translate(string language, string key);
}