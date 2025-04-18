namespace N2.Core.Entity;

/// <summary>
/// The DataContextFactory is used to create a new instance of the DataContext.
/// The factory is used to create a new instance, so that the instance can be disposed
/// of when it is no longer needed. The use of a factory is required when using
/// interactive components, as the components are created and disposed at a much
/// lower frequency than you might expect. This occurs in frameworks like Blazor due to
/// an optimization that allows the components to be reused, rather than being created.
/// </summary>
public interface ICoreDataContextFactory<TContext> where TContext : ICoreDataContext
{
    Task<TContext> CreateAsync(string connectionName);

    Task<TContext> CreateAsync();
}

public interface ICoreDataContext : IDisposable
{
    string CurrentDatabaseName { get; }

    void AddChangeLog(IChangeLog changeLog);

    void AddChangeLog<T>(Guid publicId, string message, Guid userId, string userName) where T : class;

    void AddRecord<T>(T model) where T : class;

    Task<(int resultCode, string message)> DeleteAsync<T>(Guid publicId) where T : class;

    Task<T?> FindRecordAsync<T>(Guid publicId) where T : class;

    Task<List<KeyValuePair<string, string>>> GetSelectListAsync(string tableName);

    Task<(int code, string message)> SaveChangesAsync();

    IQueryable<IChangeLog> ChangeLogs { get; }
}