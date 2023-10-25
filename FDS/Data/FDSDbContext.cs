using Microsoft.EntityFrameworkCore;

namespace FDS.Data
{
    public class FDSDbContext : DbContext
    {

        public FDSDbContext(DbContextOptions options) : base(options) { }


        public DbSet<User> Users { get; set; }


    }
}
