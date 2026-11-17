namespace N2.Core.Identity;

public interface IUserContextFactory
{
    Task<IUserContext> Create(CancellationToken cancellationToken);
}
