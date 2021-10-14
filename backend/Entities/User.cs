using System;
using MongoDB.Bson.Serialization.Attributes;
using MyWeb.Dtos;

namespace MyWeb.Entities
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        public User(string username, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = password;
        }

        [BsonId]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UserDto asDto()
        {
            return new UserDto(this.Id, this.Username, this.Password);
        }
    }
}
