namespace N2.Core.Entity;

public interface IChangeLog
{
    DateTime Created { get; set; }
    Guid CreatedBy { get; set; }
    string CreatedByName { get; set; }
    int Id { get; set; }
    Guid LogRecordId { get; set; }
    string Message { get; set; }
    Guid ReferenceId { get; set; }
    string TableName { get; set; }
}