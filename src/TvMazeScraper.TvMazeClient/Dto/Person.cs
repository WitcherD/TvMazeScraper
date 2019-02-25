using Newtonsoft.Json;

namespace TvMazeScraper.TvMazeClient.Dto
{
    public class Person
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthday")]
        public string Birthday { get; set; }
    }
}