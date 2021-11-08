using System.Threading.Tasks;
using MyWeb.Models.Entities;

namespace MyWeb.Repositories
{
    public interface ILoginUserRepo : IGenericRepository<LoginUser>
    {
        /// <summary>
        /// Find <c>LoginUser</c> from database by <paramref name="userName"/>
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="isNoTracking">Is get result asNotTracking()</param>
        /// <returns>
        /// Return <c>LoginUser</c> if exist else return null
        /// </returns>
        Task<LoginUser> FindLoginUserByUserNameAsync(string userName, bool isNoTracking);
    }
}
