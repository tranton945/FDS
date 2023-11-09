using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FDS.Data
{
    public class FDSDbContext : IdentityDbContext<ApplicationUser>
    {

        public FDSDbContext(DbContextOptions<FDSDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<OldDocVer> OldDocVers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserDTO> UserDTOs { get; set; }

    }
}
