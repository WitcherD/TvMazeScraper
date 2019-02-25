using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Infrastructure.EntityConfigurations
{
    public class ShowCastTypeConfiguration : IEntityTypeConfiguration<ShowCast>
    {
        public void Configure(EntityTypeBuilder<ShowCast> builder)
        {
            builder.ToTable("ShowCast");
            builder.HasKey(i => i.ID);
            builder.Property<int>("ShowID");
            builder.Property<int>("PersonID");
        }
    }
}