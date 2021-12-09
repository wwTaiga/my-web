using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models.Dtos;
using MyWeb.Models.Entities;
using MyWeb.Services;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Abstractions.OpenIddictConstants.Claims;

namespace MyWeb.Controllers
{
    [Route("account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly ITokenService _tokenService;
        private readonly IRepoService _repoService;
        private readonly UserManager<LoginUser> _userManager;
        private readonly SignInManager<LoginUser> _signInManager;
        private readonly OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> _tokenManager;

        public AccountController(ITokenService accountService,
                IRepoService repoService,
                UserManager<LoginUser> userManager,
                SignInManager<LoginUser> signInManager,
                OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> tokenManager)
        {
            _tokenService = accountService;
            _repoService = repoService;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerDto"></param>
        /// <response code="200">Return success message</response>
        /// <response code="403">Return error messages</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> DoRegister(RegisterDto registerDto)
        {
            LoginUser newUser = new()
            {
                UserName = registerDto.userName,
                Email = registerDto.email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(newUser,
                    registerDto.password);
            if (result.Succeeded)
            {
                return Ok("User created");
            }
            else
            {
                // TODO: log
                StringBuilder sb = new();
                foreach (var error in result.Errors)
                {
                    sb.Append(error.Description);
                }
                return Ok(sb.ToString());
            }
        }

        [HttpPost("~/connect/token")]
        [AllowAnonymous]
        public async Task<IActionResult> ConnectToken()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request.IsPasswordGrantType())
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The username/password couple is invalid."
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The username/password couple is invalid."
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Create a new ClaimsPrincipal containing the claims that
                // will be used to create an id_token, a token or a code.
                var principal = await _signInManager.CreateUserPrincipalAsync(user);

                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                principal.SetScopes(new[]
                {
                    Scopes.OpenId,
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.OfflineAccess,
                    Scopes.Roles
                }.Intersect(request.GetScopes()));
                var a = DateTime.Now;

                foreach (var claim in principal.Claims)
                {
                    claim.SetDestinations(_tokenService.GetDestinations(claim, principal));
                }

                bool rememberMe = (bool)request.GetParameter("rememberMe");
                if (rememberMe)
                {
                    principal.SetRefreshTokenLifetime(TimeSpan.FromDays(15));
                }

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                // Note: if you want to automatically invalidate the refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                var user = await _userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The refresh token is no longer valid."
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                    });
                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Create a new ClaimsPrincipal containing the claims that
                // will be used to create an id_token, a token or a code.
                var principal = await _signInManager.CreateUserPrincipalAsync(user);
                foreach (var claim in principal.Claims)
                {
                    claim.SetDestinations(_tokenService.GetDestinations(claim, principal));
                }

                bool rememberMe = (bool)request.GetParameter("rememberMe");
                if (rememberMe)
                {
                    principal.SetRefreshTokenLifetime(TimeSpan.FromDays(15));
                }

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            throw new NotImplementedException("The specified grant type is not implemented.");
        }

        /// <summary>
        /// Sign out user and revoke all valid refresh token of the user.
        /// </summary>
        [HttpPost("logout")]
        public async Task<ActionResult> DoLogout()
        {
            string authId = HttpContext.User.FindFirst(Private.AuthorizationId).Value;
            await foreach (var token in _tokenManager.FindByAuthorizationIdAsync(authId))
            {
                await _tokenManager.TryRevokeAsync(token);
            }
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}
