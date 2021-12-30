using Microsoft.AspNetCore.Authorization;
using MyWeb.Models.Enums;

namespace MyWeb.Attributes
{
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
}
