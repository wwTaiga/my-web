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

        /// <summary>
        /// Get <c>LoginUser</c> from database by <paramref name="userName"/>
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="isNoTracking">Is get result asNotTracking()</param>
        /// <returns>
        /// Return <c>LoginUser</c> if exist else return null
        /// </returns>
        Task<LoginUser> GetLoginUserByUserNameAsync(string userName, bool isNoTracking);

        Task AddLoginUserAsync(LoginUser user);

        Task UpdateLoginUserAsync(LoginUser user);

        Task DeleteLoginUserAsync(Guid id);
    }
}
