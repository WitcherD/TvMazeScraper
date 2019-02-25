using Newtonsoft.Json.Converters;

namespace TvMazeScraper.Api.Dto
{
    public class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
