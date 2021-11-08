using System.Threading.Tasks;
using MyWeb.Repositories;

namespace MyWeb.Services
{
    public interface IRepoService
    {
        /// <summary>
        /// Call data context SaveChangesAsync function
        /// </summary>
        Task<int> SaveAsync();

        ILoginUserRepo LoginUser { get; }
    }
}
