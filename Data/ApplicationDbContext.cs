using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Register.Models;

namespace Register.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Club> Club { get; set; }
        public DbSet<Player> Player { get; set; }
        public DbSet<Personnel> Personnel { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<TeamPersonnel> TeamPersonnel { get; set; }
        public DbSet<TeamPlayer> TeamPlayer { get; set; }
    }
}