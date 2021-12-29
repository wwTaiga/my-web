using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeb.Attributes;
using MyWeb.Models.Dtos;
using MyWeb.Models.Entities;
using MyWeb.Models.Enums;
using MyWeb.Services;

namespace MyWeb.Controllers
{
    [Route("users")]
    [ApiController]
    [Authorize]
    [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
    public class LoginUserController : ControllerBase
    {
        private readonly IRepoService _repoService;

        public LoginUserController(IRepoService repoService)
        {
            _repoService = repoService;
        }

        [HttpGet("noau")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LoginUserDto>>> GaetAllUserAsync()
        {
            IEnumerable<LoginUser> loginUserList = await _repoService.LoginUser
                .FindAllNTAsync();

            return Ok(loginUserList.Select(loginUser => loginUser.asDto()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginUserDto>>> GetAllUserAsync()
        {
            IEnumerable<LoginUser> loginUserList = await _repoService.LoginUser
                .FindAllNTAsync();

            return Ok(loginUserList.Select(loginUser => loginUser.asDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoginUserDto>> GetUserByIdAsync(string id)
        {
            LoginUser loginUser = await _repoService.LoginUser.FindByConditionNT(
                    user => user.Id == id).FirstOrDefaultAsync();

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
            };

            _repoService.LoginUser.AddAsync(user);
            await _repoService.SaveAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLoginUserAsync(string id, UpdateLoginUserDto userDto)
        {
            var user = await _repoService.LoginUser.FindByConditionNT(user => user.Id == id)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            _repoService.LoginUser.UpdateAsync(user);
            await _repoService.SaveAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(string id)
        {
            var user = await _repoService.LoginUser.FindByConditionNT(user => user.Id == id)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                return NotFound();
            }

            _repoService.LoginUser.DeleteAsync(user);
            await _repoService.SaveAsync();

            return Ok();
        }
    }
}
