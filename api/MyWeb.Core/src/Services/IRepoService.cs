using System.Threading.Tasks;
using MyWeb.Core.Repositories;

namespace MyWeb.Core.Services
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
