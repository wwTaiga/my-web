using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyWeb.Models;

namespace MyWeb.Repositories
{
    public interface ILoginUserRepo
    {
        Task<IEnumerable<LoginUser>> GetAllLoginUserAsync();

        Task<LoginUser> GetLoginUserByIdAsync(Guid Id);

        Task AddLoginUserAsync(LoginUser user);

        Task UpdateLoginUserAsync(LoginUser user);

        Task DeleteLoginUserAsync(Guid id);
    }
}
