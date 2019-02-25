using System.Threading.Tasks;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Domain.Repositories
{
    /// <summary>
    /// Repository for <see cref="Show"/> entity
    /// </summary>
    public interface IShowsRepository
    {
        /// <summary>
        /// Get show with MAX ID
        /// </summary>
        Task<Show> GetLastShowAsync();

        /// <summary>
        /// Find show by ID
        /// </summary>
        Task<Show> GetShowAsync(int showID);

        /// <summary>
        /// Insert show row if it dosn't exist
        /// </summary>
        Task<Show> InsertIfNotExistsAsync(Show show);
    }
}
