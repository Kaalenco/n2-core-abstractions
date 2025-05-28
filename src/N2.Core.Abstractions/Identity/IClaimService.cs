
namespace N2.Core.Identity;

/// <summary>
/// <para>A claim service is used to validate the claims that a user should have
/// before a process is started or an operation is executed. The claim service
/// should be scoped to the user's request and is always initialized. For a non
/// authenticated user, the claims should derive from an anonymous user. Authenticated
/// users will have claims that describe the existence of user properties and their respective values.
/// </para>
/// <para>
/// User claims can be checked on existence, the claim value can be used in advanced 
/// claim scenarios. e.g.
/// <list type="bullet">
/// <item>A user cannot send a message if the 'AllowSendMessage' claim is not present</item>
/// <item>A user that is allowed to send a message, should also have a property claim 'email'</item>
/// <item>A user can submit a request for a refund if the 'Refund' claim is available</item>
/// <item>The value of refund, could indicate the maximum value allowed for the refund, e.g. $250,- </item>
/// </list>
/// </para>
/// <para>
/// Claimtypes should be selected from a controlled list, <seealso cref="System.Security.Claims"/>
/// or be defined in a domain specific claimset. A claim should have the format of a well formed URL
/// and preferably reference an actual page with information about the claim.
/// </para>
/// </summary>
public interface IClaimService
{
    /// <summary>
    /// This method will check if all claims are available in the current authentication context.
    /// The method will throw a ClaimException is any claim is missing.
    /// </summary>
    /// <param name="claims"></param>
    void AllClaims(params ClaimType[] claims);

    /// <summary>
    /// This method will check if any of claims is available in the current authentication context.
    /// The method will throw a ClaimException is no claims are found.
    /// </summary>
    /// <param name="claims"></param>
    void AnyClaim(params ClaimType[] claims);

    /// <summary>
    /// This method will try to find a claim of the given type. If the claim is found,
    /// the corresponding value is returned. If the claim is not found, a claim exception is raised.
    /// </summary>
    /// <typeparam name="T">The type of element that is expected.</typeparam>
    /// <param name="claim">The claim type</param>
    /// <returns></returns>
    T GetClaimValue<T>(ClaimType claim);
}
