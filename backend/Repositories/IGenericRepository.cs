using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyWeb.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Find all objects for class T
        /// </summary>
        /// <returns>
        /// Return a list of object for class T
        /// </returns>
        Task<IEnumerable<T>> FindAllAsync();

        /// <summary>
        /// Find all objects for class T AsNoTracking
        /// </summary>
        /// <returns>
        /// Return a list of object for class T
        /// </returns>
        Task<IEnumerable<T>> FindAllNTAsync();

        /// <summary>
        /// Find all objects for class T with conditions
        /// </summary>
        /// <returns>
        /// Return <c>IQueryable</c> for class T with conditions
        /// </returns>
        IQueryable<T> FindByCondition(
            Expression<Func<T, bool>> expression);

        /// <summary>
        /// Find all objects for class T with conditions AsNoTracking
        /// </summary>
        /// <returns>
        /// Return <c>IQueryable</c> for class T with conditions
        /// </returns>
        IQueryable<T> FindByConditionNT(
            Expression<Func<T, bool>> expression);

        /// <summary>
        /// Track given entity, insert entity to database when called
        /// Dbcontext SaveChangesAsync
        /// </summary>
        /// <param name="entity">object that need to add</param>
        void AddAsync(T entity);

        /// <summary>
        /// Track given entity, update entity in database when called
        /// Dbcontext SaveChangesAsync
        /// </summary>
        /// <param name="entity">object that need to update</param>
        void UpdateAsync(T entity);

        /// <summary>
        /// Track given entity, delete entity in database when called
        /// Dbcontext SaveChangesAsync
        /// </summary>
        /// <param name="entity">object that need to update</param>
        void DeleteAsync(T entity);
    }
}
