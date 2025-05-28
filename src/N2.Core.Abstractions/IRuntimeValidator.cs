namespace N2.Core
{
    /// <summary>
    /// A validator for items. A runtime validator should be re-entrant and should not maintain any state.
    /// They are typically used a singletons and should be registered in the dependency injection container as such.
    /// </summary>
    public interface IRuntimeValidator
    {
    }

    /// <summary>
    /// A validator for items. This interface is used to validate items at runtime.
    /// Specifically, it is used to validate items that are not known at compile time.
    /// Such as items that are loaded from a database or other external source, like
    /// API provided input and command requests.
    /// </summary>
    /// <remarks>
    /// A runtime validator should be re-entrant and should not maintain any state.
    /// The validator should be able to validate any item of type <typeparamref name="T"/>.
    /// They are typically used a singletons and should be registered in the
    /// dependency injection container as such.
    /// </remarks>
    public interface IRuntimeValidator<T> : IRuntimeValidator
    {
        /// <summary>
        /// Invoke the validator for the item.
        /// </summary>
        /// <param name="item">
        /// The item, which could be any type of <typeparamref name="T"/>.
        /// </param>
        void Validate(T item);
    }
}