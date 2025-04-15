namespace N2.Core;

public interface IAzureFunctionClient
{
    Task<TA?> CallAsync<TQ, TA>(string functionPath, TQ request, CancellationToken cancellationToken);
}