using System.Text.Encodings.Web;

namespace N2.Core;

public class HtmlString : IHtmlString
{
    public HtmlString()
    {
    }

    public HtmlString(string stringValue)
    {
        RawData = stringValue;
    }

    public string RawData { get; set; } = string.Empty;

    public string ToHtmlString() => HtmlEncoder.Default.Encode(RawData ?? "");
}