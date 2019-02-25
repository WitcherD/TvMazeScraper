using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using TvMazeScraper.Api.Application.Queries;
using TvMazeScraper.Api.Controllers;

namespace TvMazeScraper.Tests
{
    public class ShowsControllerTests
    {
        [Test]
        public async Task GetShowsAsync_Should_Send_QueryCommand()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var showsController = new ShowsController(mediatorMock.Object);

            // Act
            await showsController.GetShowsAsync(1, 1);

            // Assert
            mediatorMock.Verify(i => i.Send(It.Is<PaginatedShowsQuery>(j => j.Limit == 1 && j.Offset == 1), CancellationToken.None), Times.Once);
        }
    }
}
