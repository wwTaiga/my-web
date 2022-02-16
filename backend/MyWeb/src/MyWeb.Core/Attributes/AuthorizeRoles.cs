using Microsoft.AspNetCore.Authorization;
using MyWeb.Core.Models.Enums;

namespace MyWeb.Core.Attributes;

/// <summary>
/// Authorize user role
/// </summary>
public class AuthorizeRoles : AuthorizeAttribute
{
    /// <param name="roles">Required Role(s)</param>
    public AuthorizeRoles(params Role[] roles)
    {
        string allowedRoles = string.Join(",", roles);
        Roles = allowedRoles;
    }
}
