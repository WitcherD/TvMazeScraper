using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using TvMazeScraper.Api.Dto;
using TvMazeScraper.Infrastructure;

namespace TvMazeScraper.Api.Application.Queries
{
    /// <summary>
    /// Query handler for <see cref="PaginatedShowsQuery"/>
    /// </summary>
    public class PaginatedShowsQueryHandler : IRequestHandler<PaginatedShowsQuery, IList<Show>>
    {
        private readonly TvMazeDbContext _dbContext;

        /// <inheritdoc />
        public PaginatedShowsQueryHandler(TvMazeDbContext dbContext)
        {
            // Don't use DDD infrastrucure, only simple queries over dbcontext.
            // We could use Dapper or any other approach to fetch data.
            // But we already have EF context, therefore for now it's the most convenient way.

            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<IList<Show>> Handle(PaginatedShowsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _dbContext.Shows
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .Include(i => i.Casts)
                    .ThenInclude(i => i.Person)
                    .ToListAsync(cancellationToken);

            var result = entities.Select(i => new Show
            {
                ID = i.ID,
                Name = i.Name,
                Cast = i.Casts.DistinctBy(j => j.Person.ID).Select(j => new ShowCast
                {
                    ID = j.Person.ID,
                    Name = j.Person.Name,
                    Birthday = j.Person.Birthday
                }).OrderByDescending(j => j.Birthday).ToList()
            }).ToList();

            return result;
        }
    }
}
