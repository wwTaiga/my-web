using System;
using System.ComponentModel.DataAnnotations;
using MyWeb.Dtos;

namespace MyWeb.Models
{
    public class LoginUser
    {
        public LoginUser()
        {
            Id = Guid.NewGuid();
        }

        public LoginUser(string username, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = password;
        }

        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginUserDto asDto()
        {
            return new LoginUserDto(this.Id, this.Username, this.Password);
        }
    }
}
