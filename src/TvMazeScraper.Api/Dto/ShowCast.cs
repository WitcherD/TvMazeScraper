using System;
using Newtonsoft.Json;

namespace TvMazeScraper.Api.Dto
{
    /// <summary>
    /// Show cast
    /// </summary>
    public class ShowCast
    {
        /// <summary>
        /// TVMaze PersonID
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// The name of the actor
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Birthday of the actor
        /// </summary>
        [JsonProperty("birthday")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime? Birthday { get; set; }
    }
}