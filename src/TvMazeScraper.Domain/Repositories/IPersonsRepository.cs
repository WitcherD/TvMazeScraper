using System.Threading.Tasks;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Domain.Repositories
{
    /// <summary>
    /// Repository for <see cref="Person"/> entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Find person by ID
        /// </summary>
        Task<Person> GetPersonAsync(int personID);

        /// <summary>
        /// Insert person row if it dosn't exist
        /// </summary>
        Task<Person> InsertIfNotExistsAsync(Person person);
    }
}
