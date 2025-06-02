using N2.Core.Commands;

namespace N2.Core.Exceptions;

public class ConfigurationException : N2CoreException
{
    public ConfigurationException(string message) : base(ResponseStatus.PreconditionFailed, message)
    {
    }

    public ConfigurationException() : base(ResponseStatus.PreconditionFailed)
    {
    }

    public ConfigurationException(string message, Exception innerException) : base(ResponseStatus.PreconditionFailed, message, innerException)
    {
    }
}