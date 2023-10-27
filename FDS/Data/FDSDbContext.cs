using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FDS.Data
{
    public class FDSDbContext : IdentityDbContext<ApplicationUser>
    {

        public FDSDbContext(DbContextOptions<FDSDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }


    }
}
