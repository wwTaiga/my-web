using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;

namespace MyWeb.Repositories
{
    public class LoginUserRepo : ILoginUserRepo
    {
        private readonly IDataContext _context;

        public LoginUserRepo(IDataContext context)
        {
            this._context = context;
        }

        public async Task AddLoginUserAsync(LoginUser user)
        {
            _context.LoginUser.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLoginUserAsync(Guid id)
        {
            LoginUser existingLoginUser = await _context.LoginUser.FindAsync(id);

            if (existingLoginUser is null)
            {
                throw new NullReferenceException();
            }

            _context.LoginUser.Remove(existingLoginUser);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LoginUser>> GetAllLoginUserAsync()
        {
            return await _context.LoginUser.ToListAsync();
        }

        public async Task<LoginUser> GetLoginUserByIdAsync(Guid id)
        {
            LoginUser existingLoginUser = await _context.LoginUser.FindAsync(id);
            if (existingLoginUser is null)
            {
                throw new NullReferenceException();
            }

            return existingLoginUser;
        }

        public async Task<LoginUser> GetLoginUserByUserNameAsync(string userName,
                bool isNoTracking)
        {
            IQueryable<LoginUser> query = _context.LoginUser.Where(loginUser =>
                loginUser.UserName == userName
            );

            if (isNoTracking)
            {
                query = query.AsNoTracking();
            }

            LoginUser existingLoginUser = await query.FirstOrDefaultAsync();

            if (existingLoginUser is null)
            {
                throw new NullReferenceException();
            }

            return existingLoginUser;
        }

        public async Task UpdateLoginUserAsync(LoginUser loginUser)
        {
            LoginUser existingLoginUser = await _context.LoginUser.FindAsync(loginUser.Id);
            if (existingLoginUser is null)
            {
                throw new NullReferenceException();
            }

            await _context.SaveChangesAsync();
        }
    }
}
