using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Infrastructure.EntityConfigurations
{
    public class PersonTypeConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");
            builder.HasKey(i => i.ID);
            builder.Property(p => p.ID).ValueGeneratedNever();
            builder.HasMany(i => i.Casts).WithOne(i => i.Person);
        }
    }
}