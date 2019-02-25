using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using TvMazeScraper.Api.Application.Commands;
using TvMazeScraper.Domain.Repositories;
using TvMazeScraper.TvMazeClient;
using TvMazeScraper.TvMazeClient.Dto;
using Show = TvMazeScraper.Domain.Entities.Show;

namespace TvMazeScraper.Tests
{
    public class SyncTvMazeDbCommandHandlerTests
    {
        [Test]
        public async Task GetNextPageNumberAsync_EmptyStorage_Should_Return_Zero()
        {
            // Arrange
            var tvMazeServiceMock = new Mock<ITvMazeService>();
            var showsRepositoryMock = new Mock<IShowsRepository>();
            var personsRepositoryMock = new Mock<IPersonsRepository>();

            showsRepositoryMock.Setup(i => i.GetLastShowAsync()).ReturnsAsync(() => (Show)null);

            // Act
            var commandHandler = new SyncTvMazeDbCommandHandler(tvMazeServiceMock.Object, showsRepositoryMock.Object, personsRepositoryMock.Object);

            // Assert
            var result = await commandHandler.GetNextPageNumberAsync();
            Assert.AreEqual(0, result);
        }

        [TestCase(0, 0)]
        [TestCase(249, 0)]
        [TestCase(250, 1)]
        [TestCase(300, 1)]
        [TestCase(499, 1)]
        [TestCase(500, 2)]
        public async Task GetNextPageNumberAsync_Should_Return_CorrectPage(int id, int expectedPage)
        {
            // Arrange
            var tvMazeServiceMock = new Mock<ITvMazeService>();
            var showsRepositoryMock = new Mock<IShowsRepository>();
            var personsRepositoryMock = new Mock<IPersonsRepository>();

            showsRepositoryMock.Setup(i => i.GetLastShowAsync()).ReturnsAsync(() => new Show(id, "test"));

            // Act
            var commandHandler = new SyncTvMazeDbCommandHandler(tvMazeServiceMock.Object, showsRepositoryMock.Object, personsRepositoryMock.Object);

            // Assert
            var result = await commandHandler.GetNextPageNumberAsync();
            Assert.AreEqual(expectedPage, result);
        }

        [Test]
        public async Task Handle_EmptyStorage_Should_Fill_LocalDatabase()
        {
            // Arrange
            var tvMazeServiceMock = new Mock<ITvMazeService>();
            var showsRepositoryMock = new Mock<IShowsRepository>();
            var personsRepositoryMock = new Mock<IPersonsRepository>();

            tvMazeServiceMock.SetupSequence(i => i.GetShowsAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<TvMazeClient.Dto.Show>
                {
                    new TvMazeClient.Dto.Show{ID = 1, Name = "1"},
                    new TvMazeClient.Dto.Show{ID = 2, Name = "2"},
                    new TvMazeClient.Dto.Show{ID = 3, Name = "3"}
                })
                .ReturnsAsync(new List<TvMazeClient.Dto.Show>());

            tvMazeServiceMock.Setup(i => i.GetShowCastsAsync(It.IsAny<int>())).ReturnsAsync(() => new List<TvMazeClient.Dto.ShowCast>
            {
                new TvMazeClient.Dto.ShowCast{Person = new Person{ID = 1, Name = "1", Birthday = "1990-01-01"}},
                new TvMazeClient.Dto.ShowCast{Person = new Person{ID = 2, Name = "2", Birthday = "1990-01-01"}},
                new TvMazeClient.Dto.ShowCast{Person = new Person{ID = 3, Name = "3", Birthday = "1990-01-01"}},
            });

            showsRepositoryMock.Setup(i => i.GetLastShowAsync()).ReturnsAsync(() => new Show(1, "test"));

            var services = new ServiceCollection();
            services.AddMediatR(configuration => { }, new Assembly[] { typeof(SyncTvMazeDbCommand).Assembly });
            services.AddTransient(i => tvMazeServiceMock.Object);
            services.AddTransient(i => showsRepositoryMock.Object);
            services.AddTransient(i => personsRepositoryMock.Object);
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMediator>();

            // Act
            await mediator.Send(new SyncTvMazeDbCommand());
           
            // Assert
            showsRepositoryMock.Verify(i => i.InsertIfNotExistsAsync(It.Is<Show>(j => j.ID == 1)));
            showsRepositoryMock.Verify(i => i.InsertIfNotExistsAsync(It.Is<Show>(j => j.ID == 2)));
            showsRepositoryMock.Verify(i => i.InsertIfNotExistsAsync(It.Is<Show>(j => j.ID == 3)));
            personsRepositoryMock.Verify(i => i.InsertIfNotExistsAsync(It.Is<Domain.Entities.Person>(j => j.ID == 1)), Times.Exactly(3));
            personsRepositoryMock.Verify(i => i.InsertIfNotExistsAsync(It.Is<Domain.Entities.Person>(j => j.ID == 2)), Times.Exactly(3));
            personsRepositoryMock.Verify(i => i.InsertIfNotExistsAsync(It.Is<Domain.Entities.Person>(j => j.ID == 3)), Times.Exactly(3));
        }
    }
}
