using N2.Core.Commands;

namespace N2.Core.Entity;

/// <summary>
/// Generic data repository for T.
/// </summary>
public interface IDataRepository<T> : IDisposable
    where T : IRecord
{
    /// <summary>
    /// Count the number of items that are in the dataset, using the filter parameters to limit the result.
    /// </summary>
    /// <param name="filterParameters">
    /// The filter parameters.
    /// </param>
    /// <returns>
    /// An integer value representing the number of records in the result.
    /// </returns>
    int FindCount(Dictionary<string, string> filterParameters);

    /// <summary>
    /// Finds the recortds in the dataset, using the filter parameters to limit the result.
    /// </summary>
    /// <param name="filterParameters">
    /// The filter parameters.
    /// </param>
    /// <param name="p">
    /// The page to return.
    /// </param>
    /// <param name="ipp">
    /// The number of items per page in the result.
    /// </param>
    /// <returns>
    /// <![CDATA[IEnumerable<T>]]> A list of records
    /// </returns>
    IPagedResponse<T> Find(Dictionary<string, string> filterParameters, int p, int ipp);

    /// <summary>
    /// Find a single record, using the unique reference field in the repository.
    /// </summary>
    /// <param name="reference">
    /// </param>
    /// <returns>
    /// A single record or null if the reference is invalid or if the record cannot be found.
    /// </returns>
    T FindByRef(Guid reference);

    /// <summary>
    /// Find a single record, using the primary key. Records marked for deletion are automatically
    /// filtered. To find records marked for deletion, use the Find method, with a filter 'IsDeleted=true'
    /// </summary>
    /// <param name="id">
    /// The id.
    /// </param>
    /// <returns>
    /// A single record or null if the id is invalid or if the record cannot be found.
    /// </returns>
    T FindById(int id);

    /// <summary>
    /// Creates a new record. Some fields wil be ignored, such as the primary key.
    /// </summary>
    /// <param name="record">
    /// The item that is created.
    /// </param>
    /// <returns>
    /// A ResponseStatus.
    /// </returns>
    ResponseStatus Create(T record);

    /// <summary>
    /// Creates a new record and return the record as completed by the business rules. Create and
    /// return wil take more time than just the create method, as the entire create/read cycle must
    /// be completed before a response can be created.
    /// </summary>
    /// <param name="record">
    /// The item.
    /// </param>
    /// <returns>
    /// <![CDATA[Task<T>]]>
    /// </returns>
    Task<T> CreateAndReturn(T record);

    /// <summary>
    /// Updates the record.
    /// </summary>
    /// <param name="id">
    /// The primary key for the record that will be modified.
    /// </param>
    /// <param name="record">
    /// The record parameters. Some fields wil be ignored, such as the primary key.
    /// </param>
    /// <returns>
    /// A ResponseStatus.
    /// </returns>
    ResponseStatus Update(int id, T record);

    /// <summary>
    /// Updates the record and return the modified record as completed by the business rules. Update
    /// and return wil take more time than just the create method, as the entire update/read cycle
    /// must be completed before a response can be created.
    /// </summary>
    /// <param name="id">
    /// The primary key for the record that will be modified.
    /// </param>
    /// <param name="record">
    /// The record.
    /// </param>
    /// <returns>
    /// <![CDATA[Task<T>]]>
    /// </returns>
    Task<T> UpdateAndReturn(int id, T record);

    /// <summary>
    /// Deletes the record identified by its primary key.
    /// </summary>
    /// <param name="id">
    /// The id.
    /// </param>
    /// <returns>
    /// A ResponseStatus.
    /// </returns>
    ResponseStatus Delete(int id);

    /// <summary>
    /// Deletes the record identified by its reference key.
    /// </summary>
    /// <param name="reference">
    /// The id.
    /// </param>
    /// <returns>
    /// A ResponseStatus.
    /// </returns>
    ResponseStatus Delete(Guid reference);
}