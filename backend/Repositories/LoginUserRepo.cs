using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWeb.DataContexts;
using MyWeb.Models;

namespace MyWeb.Repositories
{
    public class LoginUserRepo : ILoginUserRepo
    {
        private readonly IDataContext context;

        public LoginUserRepo(IDataContext context)
        {
            this.context = context;
        }

        public async Task AddLoginUserAsync(LoginUser user)
        {
            context.LoginUser.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteLoginUserAsync(Guid id)
        {
            LoginUser existingLoginUser = await context.LoginUser.FindAsync(id);

            if (existingLoginUser is null)
            {
                throw new NullReferenceException();
            }

            context.LoginUser.Remove(existingLoginUser);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LoginUser>> GetAllLoginUserAsync()
        {
            return await context.LoginUser.ToListAsync();
        }

        public async Task<LoginUser> GetLoginUserByIdAsync(Guid id)
        {
            LoginUser existingLoginUser = await context.LoginUser.FindAsync(id);
            if (existingLoginUser is null)
            {
                throw new NullReferenceException();
            }

            return existingLoginUser;
        }

        public async Task UpdateLoginUserAsync(LoginUser loginUser)
        {
            LoginUser existingLoginUser = await context.LoginUser.FindAsync(loginUser.Id);
            if (existingLoginUser is null)
            {
                throw new NullReferenceException();
            }

            await context.SaveChangesAsync();
        }
    }
}
