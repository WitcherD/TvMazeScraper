using System.Collections.Generic;
using Newtonsoft.Json;

namespace TvMazeScraper.Api.Dto
{
    /// <summary>
    /// TV Show scraped from TVMaze
    /// </summary>
    public class Show
    {
        /// <summary>
        /// TVMaze ShowID
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }
        
        /// <summary>
        /// Title of the show
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Cast
        /// </summary>
        [JsonProperty("cast")]
        public List<ShowCast> Cast { get; set; }
    }
}
