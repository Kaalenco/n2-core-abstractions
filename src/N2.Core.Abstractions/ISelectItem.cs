namespace N2.Core;

public interface ISelectItem
{
    Guid PublicId { get; }
    string Name { get; }
    string Description { get; }
    bool Selected { get; }
}