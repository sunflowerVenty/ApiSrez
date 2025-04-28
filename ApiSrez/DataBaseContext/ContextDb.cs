using ApiSrez.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiSrez.DataBaseContext
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Games> Games { get; set; }
        public DbSet<Order> Order { get; set; }
    }
}
