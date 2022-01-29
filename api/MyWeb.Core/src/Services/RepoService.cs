using System.Threading.Tasks;
using MyWeb.Data;
using MyWeb.Repositories;

namespace MyWeb.Services
{
    public class RepoService : IRepoService
    {
        private IDataContext _context;

        public ILoginUserRepo LoginUser { get; }

        public RepoService(IDataContext context, ILoginUserRepo loginUserRepo)
        {
            _context = context;
            LoginUser = loginUserRepo;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
