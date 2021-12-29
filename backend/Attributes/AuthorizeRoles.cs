using Microsoft.AspNetCore.Authorization;
using MyWeb.Models.Enums;

namespace MyWeb.Attributes
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles(params Role[] roles)
        {
            string allowedRoles = string.Join(",", roles);
            Roles = allowedRoles;
        }
    }
}
