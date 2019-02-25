using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TvMazeScraper.Api.Application.Queries;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Infrastructure;

namespace TvMazeScraper.Tests
{
    public class PaginatedShowsQueryHandlerTests
    {
        [Test]
        public async Task Handle_PageLimit_Should_Limit_ResultList()
        {
            var options = new DbContextOptionsBuilder<TvMazeDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using (var context = new TvMazeDbContext(options))
            {
                // Arrange
                context.Shows.Add(new Show(1, "1"));
                context.Shows.Add(new Show(2, "2"));

                context.SaveChanges();

                // Act
                var queryHandler = new PaginatedShowsQueryHandler(context);
                var result = await queryHandler.Handle(new PaginatedShowsQuery(1, 0), CancellationToken.None);

                // Assert
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(1, result.First().ID);
            }
        }

        [Test]
        public async Task Handle_PageOffset_Should_Skip_FirstRows()
        {
            var options = new DbContextOptionsBuilder<TvMazeDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using (var context = new TvMazeDbContext(options))
            {
                // Arrange
                context.Shows.Add(new Show(1, "1"));
                context.Shows.Add(new Show(2, "2"));

                context.SaveChanges();

                // Act
                var queryHandler = new PaginatedShowsQueryHandler(context);
                var result = await queryHandler.Handle(new PaginatedShowsQuery(1, 1), CancellationToken.None);

                // Assert
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(2, result.First().ID);
            }
        }

        [Test]
        public async Task Handle_Cast_Should_OrderedDesc()
        {
            var options = new DbContextOptionsBuilder<TvMazeDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using (var context = new TvMazeDbContext(options))
            {
                // Arrange
                var show1 = context.Shows.Add(new Show(1, "1"));

                var person1 = context.Persons.Add(new Person(1, "1", null));
                var person2 = context.Persons.Add(new Person(2, "2", DateTime.Parse("1900-01-01")));
                var person3 = context.Persons.Add(new Person(3, "3", DateTime.Parse("2000-01-01")));

                context.Casts.Add(new ShowCast(show1.Entity, person1.Entity));
                context.Casts.Add(new ShowCast(show1.Entity, person2.Entity));
                context.Casts.Add(new ShowCast(show1.Entity, person3.Entity));

                context.SaveChanges();

                // Act
                var queryHandler = new PaginatedShowsQueryHandler(context);
                var result = await queryHandler.Handle(new PaginatedShowsQuery(1, 0), CancellationToken.None);

                // Assert
                Assert.AreEqual(3, result.First().Cast.Count);
                Assert.AreEqual(3, result.First().Cast[0].ID);
                Assert.AreEqual(2, result.First().Cast[1].ID);
                Assert.AreEqual(1, result.First().Cast[2].ID);
            }
        }

        [Test]
        public async Task Handle_Cast_Should_Distinct_DupplicatedRows()
        {
            var options = new DbContextOptionsBuilder<TvMazeDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using (var context = new TvMazeDbContext(options))
            {
                // Arrange
                var show1 = context.Shows.Add(new Show(1, "1"));
                var person1 = context.Persons.Add(new Person(1, "1", null));
                context.Casts.Add(new ShowCast(show1.Entity, person1.Entity));
                context.Casts.Add(new ShowCast(show1.Entity, person1.Entity));
                context.Casts.Add(new ShowCast(show1.Entity, person1.Entity));

                context.SaveChanges();

                // Act
                var queryHandler = new PaginatedShowsQueryHandler(context);
                var result = await queryHandler.Handle(new PaginatedShowsQuery(1, 0), CancellationToken.None);

                // Assert
                Assert.AreEqual(1, result.First().Cast.Count);
            }
        }

        [Test]
        public async Task Handle_EmptyCast_Should_Return_EmptyCast()
        {
            var options = new DbContextOptionsBuilder<TvMazeDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using (var context = new TvMazeDbContext(options))
            {
                // Arrange
                context.Shows.Add(new Show(1, "1"));
                context.SaveChanges();

                // Act
                var queryHandler = new PaginatedShowsQueryHandler(context);
                var result = await queryHandler.Handle(new PaginatedShowsQuery(1, 0), CancellationToken.None);

                // Assert
                Assert.AreEqual(0, result.First().Cast.Count);
            }
        }

        [Test]
        public void Handle_TooBigLimit_Should_Throw_ArgumentException()
        {
            var options = new DbContextOptionsBuilder<TvMazeDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using (var context = new TvMazeDbContext(options))
            {
                // Arrange
                context.Shows.Add(new Show(1, "1"));
                context.SaveChanges();

                // Act Assert
                var queryHandler = new PaginatedShowsQueryHandler(context);
                Assert.ThrowsAsync<ArgumentException>(async () => await queryHandler.Handle(new PaginatedShowsQuery(Int32.MaxValue, 0), CancellationToken.None));
            }
        }

        [Test]
        public async Task Handle_TooBigOffset_Should_Return_EmptyList()
        {
            var options = new DbContextOptionsBuilder<TvMazeDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using (var context = new TvMazeDbContext(options))
            {
                // Arrange
                context.Shows.Add(new Show(1, "1"));
                context.Shows.Add(new Show(2, "2"));

                context.SaveChanges();

                // Act
                var queryHandler = new PaginatedShowsQueryHandler(context);
                var result = await queryHandler.Handle(new PaginatedShowsQuery(1, Int32.MaxValue), CancellationToken.None);

                // Assert
                Assert.AreEqual(0, result.Count);
            }
        }
    }
}
