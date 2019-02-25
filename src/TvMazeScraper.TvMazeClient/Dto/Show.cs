using Newtonsoft.Json;

namespace TvMazeScraper.TvMazeClient.Dto
{
    public class Show
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
