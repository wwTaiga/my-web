using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;

namespace MyWeb.Repositories
{
    public class LoginUserRepo : GenericRepository<LoginUser>, ILoginUserRepo
    {
        public LoginUserRepo(IDataContext context) : base(context)
        {
        }

        public async Task<LoginUser> FindLoginUserByUserNameAsync(string userName,
                bool isNoTracking)
        {
            IQueryable<LoginUser> query = Context.LoginUser.Where(loginUser =>
                loginUser.UserName == userName
            );

            if (isNoTracking)
            {
                query = query.AsNoTracking();
            }

            LoginUser existingLoginUser = await query.FirstOrDefaultAsync();

            return existingLoginUser;
        }

    }
}
