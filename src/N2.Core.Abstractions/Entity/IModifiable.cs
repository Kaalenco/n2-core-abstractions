namespace N2.Core.Entity;

public interface IModifiable
{
    bool DbItem { get; set; }
    Guid PublicId { get; set; }
    DateTime Created { get; set; }
    DateTime Modified { get; set; }
    Guid CreatedBy { get; set; }
    Guid ModifiedBy { get; set; }
}
