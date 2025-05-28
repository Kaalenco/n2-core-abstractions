namespace N2.Core.Identity;

/// <summary>
/// The controlled list of claim types in use. Extend these with care. For a set of well-known claim
/// types, see https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes
/// </summary>
public enum ClaimType
{
    /// <summary>
    /// undefined or invalid claim
    /// </summary>
    none = 0,

    /// <summary>
    /// A unique identifier for an entity, such as a user or service. Specifies the URI for a user
    /// principal name (UPN) claim; http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn
    /// </summary>
    UserName = 1,

    /// <summary>
    /// A claim that specifies the __validated__ email address of an entity,
    /// http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress
    /// </summary>
    Email = 2,

    /// <summary>
    /// The URI for a claim that specifies the role of an entity, http://schemas.microsoft.com/ws/2008/06/identity/claims/role
    /// </summary>
    Role = 3,

    /// <summary>
    /// The actor claim type, which is used to specify the entity that is acting on behalf of
    /// another entity. http://schemas.xmlsoap.org/ws/2005/05/identity/claims/actor
    /// </summary>
    Actor = 4,

    /// <summary>
    /// The URI for a claim that specifies the primary SID of an entity, usually in the form of a
    /// GUID or SID. http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid
    /// </summary>
    PrimarySid = 5,

    /// <summary>
    /// The URI for a claim that specifies the full name of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name
    /// </summary>
    GivenName = 6,

    /// <summary>
    /// The URI for a claim that specifies the locality of an entity, in the form of a culture code
    /// or locale identifier. http://schemas.xmlsoap.org/ws/2005/05/identity/claims/culture
    /// </summary>
    Culture = 7,
}