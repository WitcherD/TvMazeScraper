using System;
using System.Collections.Generic;

namespace TvMazeScraper.Domain.Entities
{
    public class Person
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public DateTime? Birthday { get; private set; }

        private List<ShowCast> _casts = new List<ShowCast>();
        public IReadOnlyCollection<ShowCast> Casts => _casts;

        private Person()
        {
        }

        public Person(int id, string name, DateTime? birthday)
        {
            ID = id;
            Name = name;
            Birthday = birthday;
        }
    }
}
