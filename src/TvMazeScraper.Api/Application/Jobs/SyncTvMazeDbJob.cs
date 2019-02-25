using System.Threading.Tasks;
using MediatR;
using TvMazeScraper.Api.Application.Commands;

namespace TvMazeScraper.Api.Application.Jobs
{
    /// <summary>
    /// background job for synchronizing our local database and TvMaze database
    /// </summary>
    public class SyncTvMazeDbJob
    {
        private readonly IMediator _mediator;

        /// <inheritdoc />
        public SyncTvMazeDbJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Entry point for the job
        /// </summary>
        public Task ExecuteAsync()
        {
            // If CommandHandler fails (e.g. TVMaze API returns HTTP 500)
            // Hangfire will repeat the execution
            return _mediator.Send(new SyncTvMazeDbCommand());
        }
    }
}
