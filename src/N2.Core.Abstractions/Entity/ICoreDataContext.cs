using N2.Core.Commands;

namespace N2.Core.Entity;

public interface ICoreDataContext
{
    string CurrentDatabaseName { get; }

    void AddChangeLog(IChangeLog changeLog);

    void AddChangeLog<T>(Guid publicId, string message, Guid userId, string userName) where T : class;

    void AddRecord<T>(T model) where T : class;

    Task<(ResponseStatus status, string message)> DeleteAsync<T>(Guid publicId) where T : class;

    Task<T?> FindRecordAsync<T>(Guid publicId) where T : class;

    Task<List<KeyValuePair<string, string>>> GetSelectListAsync(string tableName);

    IQueryable<IChangeLog> ChangeLogs { get; }
}
