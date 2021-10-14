using System;
using System.Collections.Generic;
using MyWeb.Entities;

namespace MyWeb.Repositories
{
    public interface IUserRepo
    {
        IEnumerable<User> GetAllUser();

        User GetUserById(Guid Id);

        void CreateNewUser(User user);

        void UpdateUser(User user);

        void DeleteUser(Guid id);
    }
}
