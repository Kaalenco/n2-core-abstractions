namespace N2.Core.Identity;

public interface IUserContextFactory
{
    Task<IUserContext> CreateAsync();
}
