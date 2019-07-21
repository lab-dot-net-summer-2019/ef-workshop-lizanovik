using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public sealed class SamuraiContext:DbContext
    {
        public SamuraiContext()
        {
        }

        public SamuraiContext(DbContextOptions<SamuraiContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.BattleId, s.SamuraiId });

            //modelBuilder.Entity<SamuraiBattle>()
            //    .Property(sb => sb.KillStreak);

            modelBuilder.Entity<SamuraiBattle>()
                .HasOne(sb => sb.Battle)
                .WithMany(b => b.SamuraiBattles)
                .HasForeignKey(sb => new { sb.BattleId });

            modelBuilder.Entity<SamuraiBattle>()
                .HasOne(sb => sb.Samurai)
                .WithMany(s => s.SamuraiBattles)
                .HasForeignKey(sb => new { sb.SamuraiId });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SamuraiAppDataCore;Trusted_Connection=True;", b => b.MigrationsAssembly("WebApp"));
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(
                 "Server=(localdb)\\MSSQLLocalDB;Database=SamuraiAppDataCore;Trusted_Connection=True;", b => b.MigrationsAssembly("SamuraiApp.Data"));
        }
    }
}
