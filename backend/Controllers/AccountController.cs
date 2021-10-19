using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Dtos;
using MyWeb.Models;
using MyWeb.Repositories;
using MyWeb.Services;

namespace MyWeb.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _accountService;
        private readonly ILoginUserRepo _loginUserRepo;
        private readonly UserManager<LoginUser> _userManager;
        private readonly SignInManager<LoginUser> _signInManager;

        public AccountController(IAccountService accountService, ILoginUserRepo loginUserRepo,
                UserManager<LoginUser> userManager, SignInManager<LoginUser> signInManager)
        {
            _accountService = accountService;
            _loginUserRepo = loginUserRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult> DoLogin(LoginDto loginDto)
        {
            LoginUser loginUser = await _loginUserRepo.GetLoginUserByUserNameAsync(
                loginDto.userName, true);

            if (loginUser is null)
            {
                return Ok("Invalid username or password");
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(loginUser,
                    loginDto.password, false);
            if (signInResult.Succeeded)
            {
                var tokenString = _accountService.GenerateJwtToken(loginUser.UserName);
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
                UserName = registerDto.userName,
                Email = registerDto.email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.password);
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
