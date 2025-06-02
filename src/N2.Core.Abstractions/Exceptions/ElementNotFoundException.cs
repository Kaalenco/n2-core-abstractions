using N2.Core.Commands;

namespace N2.Core.Exceptions;

public class ElementNotFoundException : N2CoreException
{
    public ElementNotFoundException(string message) : base(ResponseStatus.NotFound, message)
    {
    }

    public ElementNotFoundException() : base(ResponseStatus.NotFound)
    {
    }

    public ElementNotFoundException(string message, Exception innerException) : base(ResponseStatus.NotFound, message, innerException)
    {
    }
}
