using N2.Core.Commands;

namespace N2.Core.Entity;

public interface ICoreDataContext
{
    string CurrentDatabaseName { get; }

    void AddChangeLog(IChangeLog changeLog);

    void AddChangeLog<T>(Guid publicId, string message, Guid userId, string userName) where T : class;

    void AddRecord<T>(T model) where T : class;

    Task<(ResponseStatus status, string message)> Delete<T>(Guid publicId, CancellationToken cancellationToken) where T : class;

    Task<T?> FindRecord<T>(Guid publicId, CancellationToken cancellationToken) where T : class;

    Task<List<KeyValuePair<string, string>>> GetSelectList(string tableName, CancellationToken cancellationToken);

    IQueryable<IChangeLog> ChangeLogs { get; }
}
