using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyWeb.Dtos;
using MyWeb.Models;
using MyWeb.Repositories;

namespace MyWeb.Controllers
{
    [Route("public")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<LoginUser> userManager;
        private readonly SignInManager<LoginUser> signInManager;
        private readonly ILoginUserRepo loginUserRepo;

        public AccountController(ILoginUserRepo loginUserRepo,
                UserManager<LoginUser> userManager, SignInManager<LoginUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.loginUserRepo = loginUserRepo;
        }

        [HttpPost("login")]
        public async Task<ActionResult> DoLogin(LoginDto loginDto)
        {
            LoginUser loginUser = await loginUserRepo.GetLoginUserByUsernameAsync(
                loginDto.username);

            if (loginUser is null)
            {
                return Ok("Invalid username or password");
            }

            var signInResult = await signInManager.CheckPasswordSignInAsync(loginUser, loginDto.password, false);
            if (signInResult.Succeeded)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("HOWLONGDOYOUNEEDYOURMOTHERFUCKERPLEASELETMEPASS");
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                        new Claim(ClaimTypes.Name, loginDto.username)
                        }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });

            }
            else
            {
                return Ok("Invalid username or password");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> DoRegister(RegisterDto registerDto)
        {
            LoginUser newUser = new()
            {
                UserName = registerDto.username,
                Email = registerDto.email,
                EmailConfirmed = false
            };

            var result = await userManager.CreateAsync(newUser, registerDto.password);
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
    }
}
