using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Core.Models.Dtos;
using MyWeb.Core.Models.Entities;
using MyWeb.Core.Models.Enums;
using MyWeb.Core.Services;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MyWeb.Core.Controllers
{
    [Route("account")]
    [ApiController]
    [Authorize]
    public class AccountController : MyControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IRepoService _repoService;
        private readonly UserManager<LoginUser> _userManager;
        private readonly SignInManager<LoginUser> _signInManager;
        private readonly OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> _tokenManager;

        public AccountController(IEmailService emailService,
                ITokenService accountService,
                IRepoService repoService,
                UserManager<LoginUser> userManager,
                SignInManager<LoginUser> signInManager,
                OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> tokenManager)
        {
            _emailService = emailService;
            _tokenService = accountService;
            _repoService = repoService;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registerDto">Username, password and email</param>
        /// <response code="200">Succes create new user</response>
        /// <response code="400">Missing required input or malformed request</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> DoRegister(RegisterDto registerDto)
        {
            LoginUser newUser = new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(newUser,
                    registerDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Role.User.ToString());
                // TODO: Enable after come out better handling if user failed to verify email.
                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                // var callbackUrl = Url.Action(
                //     "ConfirmEmail", "Account",
                //     new { userId = newUser.Id, code = code },
                //     protocol: Request.Scheme);
                // await _emailService.sendEmailConfirmationEmailAsync(callbackUrl, newUser);

                // TODO: Change return format
                return Ok();
            }
            else
            {
                return Code422(result);
            }
        }

        /// <summary>
        /// Endpoint that control authentication logic.
        /// This endpoint need to use cotent type "x-www-form-urlencoded" to pass parameters.
        /// Different flow may need different parameters.
        /// Current support flow: password flow and refresh token flow.
        /// </summary>
        /// <remarks>
        /// Sample password flow request:
        /// ```
        ///     grant_type: 'password'
        ///     scope: 'openid offline_access profile roles'
        ///     username: 'username'
        ///     password: 'password'
        ///     rememberMe: true | false
        /// ```
        ///
        /// Sample refresh token flow request:
        /// ```
        ///     grant_type: 'refresh_token'
        ///     scope: 'openid offline_access profile roles'
        ///     refresh_token: 'refresh token string'
        ///     rememberMe: true | false
        /// ```
        /// </remarks>
        /// <response code="200">Return new jwt token and other request info</response>
        /// <response code="400">Missing, invalid required input or malformed request</response>
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
                    string error = "The username/password couple is invalid.";
                    if (result.IsNotAllowed)
                    {
                        if (await _userManager.CheckPasswordAsync(user, request.Password) &&
                                !await _userManager.IsEmailConfirmedAsync(user))
                        {
                            error = "Email isn't confirmed.";
                        }
                    }
                    else if (result.IsLockedOut)
                    {
                        error = "Account is locked out.";
                    }
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = error
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
        /// <response code="200">Success revoke all token of the login seesion</response>
        /// <response code="401">Missing JWT token or unauthenticate user</response>
        [HttpPost("logout")]
        public async Task<ActionResult> DoLogout()
        {
            string authId = HttpContext.User.FindFirst(Claims.Private.AuthorizationId).Value;
            await foreach (var token in _tokenManager.FindByAuthorizationIdAsync(authId))
            {
                await _tokenManager.TryRevokeAsync(token);
            }
            await _signInManager.SignOutAsync();

            return Ok();
        }

        /// <summary>
        /// Generate and send forgot password link to requested user email.
        /// </summary>
        /// <param name="email">Email that need to reset password</param>
        /// <response code="200">Success send forgot password email</response>
        /// <response code="400">Missing required fields or malformed request</response>
        /// <response code="422">Invalid inputs</response>
        [HttpPost("password/forgot")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Code422(new { UserNotFound = "Cannot find the user." });

            await _emailService.sendResetPasswordEmailAsync(user);
            return Ok();
        }

        /// <summary>
        /// Reset user password
        /// </summary>
        /// <param name="dto">Token, userId, and new password</param>
        /// <response code="200">Success reset password</response>
        /// <response code="400">Missing required fields or malformed request</response>
        /// <response code="422">Invalid inputs</response>
        [HttpPost("password/reset")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return Code422(new { UserNotFound = "Cannot find the user." });

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { success = "Password has been reset." });
            }
            else
            {
                return Code422(result);
            }
        }

        // WARNING: debug function
        // [HttpPost("test")]
        // [AllowAnonymous]
        // public async Task<ActionResult> SendEmail()
        // {
        //     var newUser = await _userManager.FindByIdAsync("71ea3f62-d3ab-4e2d-b303-ca5d58228f83");
        //     if (newUser == null)
        //         return Forbid();
        //     var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        //     var callbackUrl = Url.Action(
        //             nameof(ConfirmEmail),
        //             new { token, email = newUser.Email });
        //     await _emailService.sendEmailConfirmationEmailAsync(callbackUrl, newUser);
        //     return Ok();
        // }

        /// <summary>
        /// Confirm user email.
        /// </summary>
        /// <param name="token">Email confirmation token</param>
        /// <param name="email">Email that need to confirm</param>
        /// <response code="200">Success revoke all token of the login seesion</response>
        /// <response code="400">Missing required fields or malformed request</response>
        /// <response code="422">Invalid inputs</response>
        [HttpGet("email/confirm")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail([Required] string token,
                [Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Code422(new { UserNotFound = "Cannot find the user." });

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Code422(result);
            }
        }

        /// <summary>
        /// Check if email is exist.
        /// </summary>
        /// <param name="email">Email that need to check</param>
        /// <response code="200">Return true if email exist else return false</response>
        /// <response code="400">Missing required fields or malformed request</response>
        [HttpGet("email/is-exist")]
        [AllowAnonymous]
        public async Task<ActionResult> IsEmailExist([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Ok(new { isExist = true });
            }
            else
            {
                return Ok(new { isExist = false });
            }
        }
    }
}
