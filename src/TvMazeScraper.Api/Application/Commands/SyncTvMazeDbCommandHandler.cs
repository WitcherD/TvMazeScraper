using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Domain.Repositories;
using TvMazeScraper.TvMazeClient;

namespace TvMazeScraper.Api.Application.Commands
{
    /// <summary>
    /// Command handler for <see cref="SyncTvMazeDbCommand"/> commands
    /// </summary>
    public class SyncTvMazeDbCommandHandler : AsyncRequestHandler<SyncTvMazeDbCommand>
    {
        private readonly ITvMazeService _tvMazeService;
        private readonly IShowsRepository _showsRepository;
        private readonly IPersonsRepository _personsRepository;

        /// <inheritdoc />
        public SyncTvMazeDbCommandHandler(
            ITvMazeService tvMazeService,
            IShowsRepository showsRepository,
            IPersonsRepository personsRepository)
        {
            _tvMazeService = tvMazeService ?? throw new ArgumentNullException(nameof(tvMazeService));
            _showsRepository = showsRepository ?? throw new ArgumentNullException(nameof(showsRepository));
            _personsRepository = personsRepository ?? throw new ArgumentNullException(nameof(personsRepository));
        }

        /// <inheritdoc />
        protected override async Task Handle(SyncTvMazeDbCommand request, CancellationToken cancellationToken)
        {
            var tvMazeShows = await GetNextPageShowsAsync();
            while (tvMazeShows.Any())
            {
                foreach (var tvMazeShow in tvMazeShows)
                {
                    // Perharps another thread (or instance of service) has already added the row with same ID
                    var showEntity = await _showsRepository.GetShowAsync(tvMazeShow.ID);
                    if (showEntity == null)
                    {
                        showEntity = new Show(tvMazeShow.ID, tvMazeShow.Name);

                        // Use eventual consistency and idempotent inserts instead of UoW.   
                        // Firstly insert all the cast of the show, and only then that show.
                        // Even if the job fails it will start again and repeat the previous operation.

                        var tvMazeCasts = await _tvMazeService.GetShowCastsAsync(tvMazeShow.ID);
                        foreach (var tvMazeCast in tvMazeCasts)
                        {
                            if (tvMazeCast.Person != null)
                            {
                                var personEntity = new Person(tvMazeCast.Person.ID, tvMazeCast.Person.Name, String.IsNullOrEmpty(tvMazeCast.Person.Birthday) ? (DateTime?)null : DateTime.Parse(tvMazeCast.Person.Birthday));
                                personEntity = await _personsRepository.InsertIfNotExistsAsync(personEntity);
                                showEntity.AddCast(personEntity);
                            }
                        }

                        await _showsRepository.InsertIfNotExistsAsync(showEntity);
                    }
                }

                tvMazeShows = await GetNextPageShowsAsync();
            }
        }

        /// <summary>
        /// Download next bunch of TV shows
        /// </summary>
        public async Task<List<TvMazeClient.Dto.Show>> GetNextPageShowsAsync()
        {
            var nextPageNumber = await GetNextPageNumberAsync();
            var tvMazeShows = await _tvMazeService.GetShowsAsync(nextPageNumber);
            return tvMazeShows;
        }

        /// <summary>
        /// Get a page number for downloading the next bunch of TV shows
        /// </summary>
        public async Task<int> GetNextPageNumberAsync()
        {
            var lastShow = await _showsRepository.GetLastShowAsync();
            return (lastShow?.ID ?? 0) / 250;
        }
    }
}
