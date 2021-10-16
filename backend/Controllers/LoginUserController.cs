using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Dtos;
using MyWeb.Models;
using MyWeb.Repositories;

namespace MyWeb.Controllers
{
    [ApiController]
    [Route("users")]
    public class LoginUserController : ControllerBase
    {
        private readonly ILoginUserRepo loginUserRepo;

        public LoginUserController(ILoginUserRepo userRepo)
        {
            this.loginUserRepo = userRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginUserDto>>> GetAllUserAsync()
        {
            IEnumerable<LoginUser> loginUserList = await loginUserRepo.GetAllLoginUserAsync();

            return Ok(loginUserList.Select(loginUser => loginUser.asDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoginUserDto>> GetUserByIdAsync(Guid id)
        {
            LoginUser loginUser = await loginUserRepo.GetLoginUserByIdAsync(id);

            if (loginUser is null)
            {
                return NotFound();
            }

            return Ok(loginUser.asDto());
        }

        [HttpPost]
        public async Task<ActionResult> CreateLoginUserAsync(AddLoginUserDto userDto)
        {
            LoginUser user = new()
            {
                Username = userDto.Username,
                Password = userDto.Password
            };

            await loginUserRepo.AddLoginUserAsync(user);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLoginUserAsync(Guid id, UpdateLoginUserDto userDto)
        {
            var user = await loginUserRepo.GetLoginUserByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            user.Username = userDto.Username;
            user.Password = userDto.Password;
            await loginUserRepo.UpdateLoginUserAsync(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            LoginUser user = await loginUserRepo.GetLoginUserByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            await loginUserRepo.DeleteLoginUserAsync(id);

            return Ok();
        }
    }
}
