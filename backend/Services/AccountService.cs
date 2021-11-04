using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyWeb.Models;

namespace MyWeb.Services
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _config;

        public AccountService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(LoginUser loginUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, loginUser.Id),
                        new Claim(ClaimTypes.Name, loginUser.UserName),
                        new Claim(ClaimTypes.Email, loginUser.Email)
                    }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
