namespace TvMazeScraper.Domain.Entities
{
    public class ShowCast
    {
        public int ID { get; private set; }
        
        public Show  Show { get; private set; }

        public Person Person { get; private set; }

        // Character, etc

        private ShowCast()
        {
        }

        public ShowCast(Show show, Person person)
        {
            Show = show;
            Person = person;
        }
    }
}
