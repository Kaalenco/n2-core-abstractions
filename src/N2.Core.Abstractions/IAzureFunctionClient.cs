namespace N2.Core;

public interface IAzureFunctionClient
{
    Task<TA?> CallFunction<TQ, TA>(string functionPath, TQ request, CancellationToken cancellationToken);
}