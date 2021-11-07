using System.Collections.Generic;
using System.Security.Claims;

namespace MyWeb.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Get claim destinations to access and identity token based on claims
        /// type.
        /// <br />
        /// By Default, claims are NOT automaticcaly included in access token
        /// and identity token.
        /// </summary>
        /// <param name="claim">
        /// Name value pair that represents what the subject is
        /// </param>
        /// <param name="principal">
        /// An Identity represents the user
        /// </param>
        /// <returns>
        /// A collection of token name where the claim should attached to.
        /// </returns>
        IEnumerable<string> GetDestinations(Claim claim,
            ClaimsPrincipal principal);
    }
}
