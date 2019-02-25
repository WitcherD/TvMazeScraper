using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Infrastructure.EntityConfigurations
{
    public class ShowTypeConfiguration : IEntityTypeConfiguration<Show>
    {
        public void Configure(EntityTypeBuilder<Show> builder)
        {
            builder.ToTable("Show");
            builder.HasKey(i => i.ID);
            builder.Property(p => p.ID).ValueGeneratedNever();
            builder.HasMany(i => i.Casts).WithOne(i => i.Show);
        }
    }
}
