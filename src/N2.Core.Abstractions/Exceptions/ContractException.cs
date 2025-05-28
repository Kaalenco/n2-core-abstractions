using N2.Core.Commands;

namespace N2.Core.Exceptions;

/// <summary>
/// The contract exception.
/// </summary>
public class ContractException : N2CoreException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContractException" /> class.
    /// </summary>
    public ContractException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContractException" /> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public ContractException(string message) : base(ResponseStatus.NotAcceptable, message)
    {
    }
}
