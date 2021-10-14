using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Dtos;
using MyWeb.Entities;
using MyWeb.Repositories;

namespace MyWeb.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo userRepo;

        public UserController(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        [HttpGet]
        public IEnumerable<UserDto> GetAllUser()
        {
            IEnumerable<UserDto> userDtoList = userRepo.GetAllUser().Select(user =>
                    new UserDto(user.Id, user.Username, user.Password));

            return userDtoList;
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(Guid id)
        {
            User user = userRepo.GetUserById(id);

            if (user is null)
            {
                return NotFound();
            }

            return user.asDto();
        }

        [HttpPost]
        public ActionResult<UserDto> CreateUser(CreateUserDto userDto)
        {
            User user = new()
            {
                Username = userDto.Username,
                Password = userDto.Password
            };

            userRepo.CreateNewUser(user);

            return CreatedAtAction(nameof(GetUserById),
                    new { id = user.Id },
                    user.asDto());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(Guid id, UpdateUserDto userDto)
        {
            var user = userRepo.GetUserById(id);

            if (user is null)
            {
                return NotFound();
            }

            user.Username = userDto.Username;
            user.Password = userDto.Password;
            userRepo.UpdateUser(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(Guid id)
        {
            User user = userRepo.GetUserById(id);

            if (user is null)
            {
                return NotFound();
            }

            userRepo.DeleteUser(id);

            return Ok();
        }
    }
}
