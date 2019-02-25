using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Domain.Repositories;
using TvMazeScraper.Infrastructure.Extensions;

namespace TvMazeScraper.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class EfCoreShowsRepository : IShowsRepository
    {
        private readonly TvMazeDbContext _dbContext;

        /// <inheritdoc />
        public EfCoreShowsRepository(TvMazeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public Task<Show> GetLastShowAsync()
        {
            return _dbContext.Shows.OrderByDescending(i => i.ID).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task<Show> GetShowAsync(int showID)
        {
            return _dbContext.Shows.FirstOrDefaultAsync(i => i.ID == showID);
        }

        /// <inheritdoc />
        public async Task<Show> InsertIfNotExistsAsync(Show show)
        {
            var existsingShow = await GetShowAsync(show.ID);
            if (existsingShow == null)
            {
                _dbContext.Shows.Add(show);
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex) when (ex.IsSqlUniqueConstraint())
                {
                    // Ignore unique constraint exceptions (e.g. PK)S
                }
            }
            return show;
        }
    }
}
