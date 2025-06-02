namespace N2.Core;

public interface ITranslator
{
    /// <summary>
    /// Gets the current language for texts.
    /// </summary>
    string Language { get; }

    /// <summary>
    /// Get Text for the current language, using a specific context.
    /// </summary>
    /// <param name="pageContext">A page or form context (e.g. logon page).</param>
    /// <param name="key">The key for the text. This could be a default text, but a code would suffice.</param>
    /// <returns>The text substitution for the current language.</returns>
    string GT(string pageContext, string key);

    /// <summary>
    /// Get Text for the current language.
    /// </summary>
    /// <param name="key">The key for the text. This could be a default text, but a code would suffice.</param>
    /// <returns>The text substitution for the current language.</returns>
    string GT(string key);

    /// <summary>
    /// Translate a text for a specific language and context.
    /// </summary>
    /// <param name="language">The language code.</param>
    /// <param name="pageContext">A page or form context (e.g. logon page).</param>
    /// <param name="key">The key for the text. This could be a default text, but a code would suffice.</param>
    /// <returns>The text substitution for the current language.</returns>
    string Translate(string language, string pageContext, string key);

    /// <summary>
    /// Translate a text for a specific language and key.
    /// </summary>
    /// <param name="language">The language code.</param>
    /// <param name="key">The key for the text. This could be a default text, but a code would suffice.</param>
    /// <returns>The text substitution for the current language.</returns>
    string Translate(string language, string key);
}