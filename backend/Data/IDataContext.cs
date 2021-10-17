using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWeb.Models;

namespace MyWeb.Data
{
    public interface IDataContext
    {
        DbSet<LoginUser> LoginUser { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
