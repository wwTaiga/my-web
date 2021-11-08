using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWeb.Models.Entities;

namespace MyWeb.Data
{
    public interface IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<T> Set<T>() where T : class;

        DbSet<LoginUser> LoginUser { get; set; }

    }
}
