using System.Text.Encodings.Web;

namespace N2.Core.Identity;

public class UserSelectItem : ICloneable, IHtmlString
{
    public Guid Key { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? ImagePath { get; set; }

    public string ToHtmlString() => HtmlEncoder.Default.Encode(DisplayName ?? Email ?? "");

    public bool RemoveThisRecord { get; set; }

    public object Clone()
    {
        return new UserSelectItem
        {
            Key = Key,
            DisplayName = DisplayName,
            Email = Email,
            ImagePath = ImagePath
        };
    }
}