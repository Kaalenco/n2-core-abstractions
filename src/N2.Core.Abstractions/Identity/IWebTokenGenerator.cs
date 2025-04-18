namespace N2.Core.Identity;

public interface IWebTokenGenerator
{
    string GenerateWebToken(IUserContext userContext, int timeoutInMinutes);
}