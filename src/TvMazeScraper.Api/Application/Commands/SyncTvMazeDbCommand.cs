using MediatR;

namespace TvMazeScraper.Api.Application.Commands
{
    /// <summary>
    /// Synchronize our local database (with the scraped data) and TvMaze database
    /// </summary>
    public class SyncTvMazeDbCommand : IRequest
    {
    }
}
