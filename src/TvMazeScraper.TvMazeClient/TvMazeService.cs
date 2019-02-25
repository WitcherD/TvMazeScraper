using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TvMazeScraper.TvMazeClient.Dto;

namespace TvMazeScraper.TvMazeClient
{
    /// <inheritdoc />
    public class TvMazeService : ITvMazeService
    {
        private readonly HttpClient _client;

        /// <inheritdoc />
        public TvMazeService(HttpClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<List<Show>> GetShowsAsync(int page)
        {
            var response = await _client.GetAsync($"shows?page={page}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<Show>();

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Show>>(stringContent);
        }

        /// <inheritdoc />
        public async Task<List<ShowCast>> GetShowCastsAsync(int showID)
        {
            var response = await _client.GetAsync($"shows/{showID}/cast");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return new List<ShowCast>();

            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ShowCast>>(stringContent);
        }
    }
}
