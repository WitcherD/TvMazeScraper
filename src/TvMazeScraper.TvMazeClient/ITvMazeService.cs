using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.TvMazeClient.Dto;

namespace TvMazeScraper.TvMazeClient
{
    /// <summary>
    /// TVMaze client 
    /// </summary>
    public interface ITvMazeService
    {
        /// <summary>
        /// A list of shows in TVMaze database
        /// </summary>
        Task<List<Show>> GetShowsAsync(int page);

        /// <summary>
        /// A list of main cast for a show.
        /// </summary>
        Task<List<ShowCast>> GetShowCastsAsync(int showID);
    }
}
