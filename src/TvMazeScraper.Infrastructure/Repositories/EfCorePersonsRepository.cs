using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Domain.Repositories;
using TvMazeScraper.Infrastructure.Extensions;

namespace TvMazeScraper.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class EfCorePersonsRepository : IPersonsRepository
    {
        private readonly TvMazeDbContext _dbContext;

        /// <inheritdoc />
        public EfCorePersonsRepository(TvMazeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public Task<Person> GetPersonAsync(int personID)
        {
            return _dbContext.Persons.FirstOrDefaultAsync(i => i.ID == personID);
        }

        /// <inheritdoc />
        public async Task<Person> InsertIfNotExistsAsync(Person person)
        {
            var existsingPerson = await GetPersonAsync(person.ID);
            if (existsingPerson == null)
            {
                _dbContext.Persons.Add(person);
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException ex) when (ex.IsSqlUniqueConstraint())
                {
                    // Ignore unique constraint exceptions (e.g. PK)
                }
            }
            return person;
        }
    }
}
