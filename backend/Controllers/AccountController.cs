using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Dtos;
using MyWeb.Models;
using MyWeb.Services;

namespace MyWeb.Controllers
{
    [Route("account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _accountService;
        private readonly IRepoService _repoService;
        private readonly UserManager<LoginUser> _userManager;
        private readonly SignInManager<LoginUser> _signInManager;

        public AccountController(IAccountService accountService,
                IRepoService repoService,
                UserManager<LoginUser> userManager,
                SignInManager<LoginUser> signInManager)
        {
            _accountService = accountService;
            _repoService = repoService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> DoLogin(LoginDto loginDto)
        {
            LoginUser loginUser = await _repoService.LoginUser
                .FindLoginUserByUserNameAsync(loginDto.userName, true);

            if (loginUser is null)
            {
                var response = new
                {
                    status = "fail",
                    error = new
                    {
                        code = 404,
                        message = "Invalid username or password"
                    }
                };
                return Ok(response);
            }

            var signInResult = await _signInManager
                .PasswordSignInAsync(loginUser, loginDto.password, false, false);
            if (signInResult.Succeeded)
            {
                var tokenString = _accountService.GenerateJwtToken(loginUser);

                Response.Cookies.Append("jwt", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                });

                var response = new
                {
                    status = "success",
                    data = new
                    {
                        Token = tokenString
                    }
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    status = "fail",
                    error = new
                    {
                        code = 404,
                        message = "Invalid username or password"
                    }
                };
                return Ok(response);
            }
        }

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

        [HttpPost("logout")]
        public async Task<ActionResult> DoLogout()
        {
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(5),
            });
            await _signInManager.SignOutAsync();

            return Ok("Success");
        }
    }
}
