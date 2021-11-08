using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;

namespace MyWeb.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected IDataContext Context;

        public GenericRepository(IDataContext context)
        {
            Context = context;
        }

        public void AddAsync(T entity)
        {
            Context.Set<T>().AddAsync(entity);
        }

        public void DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllNTAsync()
        {
            return await Context.Set<T>().AsNoTracking().ToListAsync();
        }

        public IQueryable<T> FindByCondition(
            Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression);
        }

        public IQueryable<T> FindByConditionNT(
            Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression).AsNoTracking();
        }
        public void UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);
        }
    }
}
