using N2.Core.Commands;

namespace N2.Core.Exceptions;

public class OperationException : N2CoreException
{
    public OperationException(string message) : base(ResponseStatus.MethodNotAllowed, message)
    {
    }

    public OperationException() : base(ResponseStatus.MethodNotAllowed)
    {
    }

    public OperationException(string message, Exception innerException) : base(ResponseStatus.MethodNotAllowed, message, innerException)
    {
    }
}
