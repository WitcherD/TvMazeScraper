using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Infrastructure.EntityConfigurations;

namespace TvMazeScraper.Infrastructure
{
    public class TvMazeDbContext : DbContext
    {
        public DbSet<Show> Shows { get; set; }

        public DbSet<ShowCast> Casts { get; set; }

        public DbSet<Person> Persons { get; set; }

        public TvMazeDbContext(DbContextOptions<TvMazeDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ShowCastTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ShowTypeConfiguration());
        }
    }
}
