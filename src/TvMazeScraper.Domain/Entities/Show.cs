using System.Collections.Generic;

namespace TvMazeScraper.Domain.Entities
{
    public class Show
    {
        public int ID { get; private set; }

        public string Name { get; private set; }

        private List<ShowCast> _casts = new List<ShowCast>();
        public IReadOnlyCollection<ShowCast> Casts => _casts;

        private Show()
        {
        }

        public Show(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public void AddCast(Person person)
        {
            _casts.Add(new ShowCast(this, person));
        }
    }
}
