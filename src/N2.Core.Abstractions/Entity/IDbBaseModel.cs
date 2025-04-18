namespace N2.Core.Entity;

public interface IDbBaseModel
{
    int Id { get; set; }
    Guid PublicId { get; set; }
    DateTime Created { get; set; }
    Guid CreatedBy { get; set; }
    bool IsRemoved { get; set; }
    DateTime Modified { get; set; }
    Guid ModifiedBy { get; set; }
    DateTime? Removed { get; set; }

    /// <summary>
    /// Add a calculated field for search purposes.
    /// </summary>
    string SearchField { get; }

    T MapTo<T>() where T: class, IBasicListModel, new();
}
