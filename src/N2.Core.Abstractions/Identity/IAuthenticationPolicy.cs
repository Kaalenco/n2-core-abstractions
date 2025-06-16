
namespace N2.Core.Identity;

/// <summary>
/// Rules for engagement.
/// </summary>
public interface IAuthenticationPolicy
{
    /// <summary>
    /// Check if the current user or process is allowed to add new elements of the type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>True if allowed, false if not.</returns>
    bool AllowAdd<T>();

    /// <summary>
    /// Check if the current user or process is allowed to remove existing elements of the type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>True if allowed, false if not.</returns>
    bool AllowRemove<T>();

    /// <summary>
    /// Check if the current user or process is allowed to mofidy elements of the type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>True if allowed, false if not.</returns>
    bool AllowEdit<T>();
}