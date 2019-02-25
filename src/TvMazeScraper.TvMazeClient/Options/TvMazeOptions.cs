using System;

namespace TvMazeScraper.TvMazeClient.Options
{
    public class TvMazeOptions
    {
        public string BaseAddress { get; set; } = "http://api.tvmaze.com";

        public TimeSpan RetryTimeout { get; set; } = TimeSpan.FromSeconds(10);
    }
}
