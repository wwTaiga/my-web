using Microsoft.EntityFrameworkCore;
using MyWeb.Models;

namespace MyWeb.DataContexts
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<LoginUser> LoginUser { get; set; }
    }
}
