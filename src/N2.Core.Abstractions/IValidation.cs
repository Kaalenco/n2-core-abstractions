using System.Collections.ObjectModel;

namespace N2.Core;

public interface IValidation
{
    bool Valid { get; }
    public ReadOnlyCollection<ValidationResult> Results { get; }
}
