using System.Threading.Tasks;
using MyWeb.Core.Data;
using MyWeb.Core.Repositories;

namespace MyWeb.Core.Services;

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
