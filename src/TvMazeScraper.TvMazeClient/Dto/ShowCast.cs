using Newtonsoft.Json;

namespace TvMazeScraper.TvMazeClient.Dto
{
    public class ShowCast
    {
        [JsonProperty("person")]
        public Person Person { get; set; }
    }
}
