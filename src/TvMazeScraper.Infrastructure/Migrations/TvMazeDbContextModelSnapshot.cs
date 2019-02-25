﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TvMazeScraper.Infrastructure;

namespace TvMazeScraper.Infrastructure.Migrations
{
    [DbContext(typeof(TvMazeDbContext))]
    partial class TvMazeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TvMazeScraper.Domain.Entities.Person", b =>
                {
                    b.Property<int>("ID");

                    b.Property<DateTime?>("Birthday");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("TvMazeScraper.Domain.Entities.Show", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Show");
                });

            modelBuilder.Entity("TvMazeScraper.Domain.Entities.ShowCast", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PersonID");

                    b.Property<int>("ShowID");

                    b.HasKey("ID");

                    b.HasIndex("PersonID");

                    b.HasIndex("ShowID");

                    b.ToTable("ShowCast");
                });

            modelBuilder.Entity("TvMazeScraper.Domain.Entities.ShowCast", b =>
                {
                    b.HasOne("TvMazeScraper.Domain.Entities.Person", "Person")
                        .WithMany("Casts")
                        .HasForeignKey("PersonID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TvMazeScraper.Domain.Entities.Show", "Show")
                        .WithMany("Casts")
                        .HasForeignKey("ShowID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}